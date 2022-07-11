using Newtonsoft.Json;
using ORMTest.WPF.Models.SqlSugar;
using RazorEngine;
using RazorEngine.Templating;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Services
{
    public static class SqlSugarService
    {
        public static string DBPath = Environment.CurrentDirectory + @"\DataBase\db.sqlite";
        public static string SqliteConnectionString = @"DataSource=" + DBPath;
        private static string PGConnectionString = "PORT=5433;DATABASE=ORMTestSqlSugar;HOST=localhost;PASSWORD=hnddzy;USER ID=hnddzy";
        private static SqlSugarScope DB = new SqlSugarScope(new ConnectionConfig()
        {
            ConnectionString = SqliteConnectionString,
            DbType = DbType.Sqlite,
            IsAutoCloseConnection = true,
            //MoreSettings = new ConnMoreSettings()
            //{
            //    PgSqlIsAutoToLower = false//区分大小写，规范默认不区分大小写
            //}
            //?可null支持
            ConfigureExternalServices = new ConfigureExternalServices
            {
                EntityService = (c, p) =>
                {
                    // int?  decimal?这种 isnullable=true
                    if (c.PropertyType.IsGenericType &&
                    c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        p.IsNullable = true;
                    }
                }
            }
        });
        public static void Test()
        {
            CodeFirst();
            DbFirst1();
            DbFirst2();
            DbFirst3();
            QueryAll();
            QueryWhere();
            QuerySelect1();
            QuerySelect2();
            QueryPage();
            QueryComplex1();
            QueryComplex2();
            QuerySql();
            Insert();
            Update();
            Delete();
        }
        /// <summary>
        /// 自动建表
        /// </summary>
        public static void CodeFirst()
        {
            if (!System.IO.File.Exists(DBPath))
            {
                if (DB.DbMaintenance.CreateDatabase())
                {
                    DB.CodeFirst.InitTables(typeof(InvType));
                    DB.CodeFirst.InitTables(typeof(InvGroup));

                    //初始化数据
                    var json = System.IO.File.ReadAllText("types.json");
                    var types = JsonConvert.DeserializeObject<List<InvType>>(json);
                    DB.Insertable(types).ExecuteCommand();
                    var json2 = System.IO.File.ReadAllText("groups.json");
                    var groups = JsonConvert.DeserializeObject<List<InvGroup>>(json2);
                    DB.Insertable(groups).ExecuteCommand();
                    DB.Ado.CommitTran();
                }
                //自动批量
                //var ls = typeof(InvType).Assembly.GetTypes().Where(p => p.FullName.Contains("Models.SqlSugar")).ToList();
                //ls.ForEach(p =>
                //{
                //    DB.CodeFirst.InitTables(p);
                //});
            }
        }
        public static void QueryAll()
        {
            var types = DB.Queryable<InvType>().ToList();
            var groups = DB.Queryable<InvGroup>().ToList();
            //var json = JsonConvert.SerializeObject(types);
            //System.IO.File.WriteAllText("types.json", json);
            //json = JsonConvert.SerializeObject(groups);
            //System.IO.File.WriteAllText("groups.json", json);
        }
        public static void QueryWhere()
        {
            var ls = DB.Queryable<InvType>().Where(p => p.GroupID == 6).ToList();
        }
        public static void QuerySelect1()
        {
            var ls = DB.Queryable<InvType>().Where(p => p.GroupID == 6).Select(p => p.TypeID).ToList();
        }
        public static void QuerySelect2()
        {
            var ls = DB.Queryable<InvType>().Where(p => p.GroupID == 6).Select(p => new { p.TypeID, p.GroupID }).ToList();
            
        }
        public static void QueryPage()
        {
            var ls = DB.Queryable<InvType>().ToPageList(1,10).ToList();
        }
        public static void QueryComplex1()
        {
            var ls = DB.Queryable<InvType>()
                .InnerJoin<InvGroup>((t,g)=>t.GroupID == g.GroupID)
                .Select((t, g) => new { Type = t, Group = g })
                .ToList();
        }
        public static void QueryComplex2()
        {
            var ls = DB.Queryable<InvType>()
                .LeftJoin<InvGroup>((t, g) => t.GroupID == g.GroupID)
                .Select((t, g) => new { Type = t, Group = g })
                .ToList();
        }
        public static void QuerySql()
        {
            var ls = DB.Ado.SqlQuery<InvType>("SELECT * FROM \"invtype\" WHERE \"GroupID\" = 6");
        }
        public static void Insert()
        {
            InvType invType = new InvType();
            invType.TypeID = 5233;
            invType.TypeName = "5233";
            DB.Insertable(invType).ExecuteCommand();
            //db.Insertable(List<实体>).ExecuteCommand()  
            //db.Insertable(List<实体>).UseParameter().ExecuteCommand()
            //DB.Fastest<RealmAuctionDatum>().BulkCopy(GetList());
            //DB.Fastest<InvType>().BulkCopy(new List<InvType>());
        }

        public static void Update()
        {
            var tpye = DB.Queryable<InvType>().ToList().Last();
            tpye.TypeName = "5230";
            DB.Updateable(tpye).ExecuteCommand();
            DB.Updateable(new List<InvType>());
        }

        public static void Delete()
        {
            var tpye = DB.Queryable<InvType>().ToList().Last();
            DB.Deleteable(tpye).ExecuteCommand();
            DB.Deleteable<InvType>(p => p.TypeID == 2);
            //DB.Deleteable(new List<InvType>()).ExecuteCommand();
        }

        /// <summary>
        /// 从数据库表创建类文件
        /// </summary>
        public static void DbFirst1()
        {
            DB.DbFirst.CreateClassFile("DBFirst\\Sugar1", "Models");
            //db.DbFirst.Where(p=>p.StartsWith("inv", StringComparison.InvariantCultureIgnoreCase)).IsCreateAttribute().CreateClassFile("D:\\DBFirst\\Sugar3", "Models");
        }
        /// <summary>
        /// 从数据库表创建类文件
        /// </summary>
        public static void DbFirst2()
        {
            DB.DbFirst.IsCreateAttribute().CreateClassFile("DBFirst\\Sugar2", "Models");
        }
        /// <summary>
        /// 从数据库表创建类文件
        /// </summary>
        public static void DbFirst3()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = PGConnectionString,
                DbType = DbType.PostgreSQL,
                IsAutoCloseConnection = true,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    RazorService = new RazorService()
                }
            });
            var templte = RazorFirst.DefaultRazorClassTemplate;//这个是自带的，这个模版可以修改
            db.DbFirst.UseRazorAnalysis(templte).CreateClassFile("DBFirst\\Sugar3");
        }
    }

    public class RazorService : IRazorService
    {
        public List<KeyValuePair<string, string>> GetClassStringList(string razorTemplate, List<RazorTableInfo> model)
        {
            if (model != null && model.Any())
            {
                var result = new List<KeyValuePair<string, string>>();
                foreach (var item in model)
                {
                    try
                    {
                        item.ClassName = item.DbTableName;//格式化类名
                        string key = "RazorService.GetClassStringList" + razorTemplate.Length;
                        var classString = Engine.Razor.RunCompile(razorTemplate, key, item.GetType(), item);
                        result.Add(new KeyValuePair<string, string>(item.ClassName, classString));
                    }
                    catch (Exception ex)
                    {
                        new Exception(item.DbTableName + " error ." + ex.Message);
                    }
                }
                return result;
            }
            else
            {
                return new List<KeyValuePair<string, string>>();
            }
        }
    }
}
