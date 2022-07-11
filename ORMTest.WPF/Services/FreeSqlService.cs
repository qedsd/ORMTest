using ORMTest.WPF.Models.FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Services
{
    public static class FreeSqlService
    {
        static readonly string PgConnectionString = "PORT=5433;DATABASE=ORMTestFSql;HOST=localhost;PASSWORD=hnddzy;USER ID=hnddzy";


        static readonly IFreeSql Fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.PostgreSQL, PgConnectionString)
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                .Build(); //请务必定义成 Singleton 单例模式

        public static void CodeFirst()
        {
            try
            {
                //手动同步实体结构
                //如需自动,启用UseAutoSyncStructure(true)即可，在CRUD时到相应表时会自动生成
                Fsql.CodeFirst.SyncStructure<InvType>();
            }
            catch (Exception ex)
            {

            }
        }
        public static void Test()
        {
            CodeFirst();
            QueryAll();
            QueryWhere1();
            QueryWhere2();
            QuerySelect1();
            QuerySelect2();
            QueryPage();
            QueryComplex();
            QuerySql();
            Insert();
            Update();
            Delete();
        }
        public static List<InvType> QueryAll()
        {
            return Fsql.Select<InvType>().ToList();
        }

        public static List<InvType> QueryWhere1()
        {
            return Fsql.Select<InvType>().Where(p=>p.GroupID == 6).ToList();
        }

        public static List<InvType> QueryWhere2()
        {
            //FreeSql.Extensions.Linq包提供的IQueryable方法
            IQueryable<InvType> queryable = Fsql.Select<InvType>().AsQueryable();
            queryable.Where(p => p.GroupID == 6).ToList();
            return (from a in Fsql.Select<InvType>()
                    where a.GroupID == 6
                    select a).ToList();
        }

        public static List<int> QuerySelect1()
        {
            return (from a in Fsql.Select<InvType>()
                    where a.GroupID == 6
                    select a.TypeID).ToList();
            //Fsql.Select<InvType>().Where(p => p.GroupID == 6).Select(p => new { p.TypeID, p.GroupID }).ToList();
        }

        public static Dictionary<int, int> QuerySelect2()
        {
            var t =  (from a in Fsql.Select<InvType>()
                    where a.GroupID == 6
                    select new {a.TypeID,a.GroupID}).ToList();
            Dictionary<int, int> result = new Dictionary<int, int>();
            t.ForEach(p =>
            {
                result.Add(p.TypeID, (int)p.GroupID);
            });
            return result;
        }
        public static List<InvType> QueryPage()
        {
            return Fsql.Select<InvType>().Where(p=>p.TypeID > 1).Count(out var total).Page(1,2).ToList();
        }

        public static void QueryComplex()
        {
            var t = Fsql.Select<InvType, InvGroup>()
                .LeftJoin((a, b) => a.GroupID == b.GroupID)
                .Where((a, b) => a.GroupID != null)
                .ToList((a, b) => new { a, b });
        }

        public static void QuerySql()
        {
            var t = Fsql.Select<InvType>().WithSql("SELECT * FROM \"InvType\"").ToDataTable("\"TypeID\",\"GroupID\"");
        }

        public static void Insert()
        {
            var maxId = Fsql.Select<InvType>().Max(p => p.TypeID);
            InvType invType = new InvType()
            {
                TypeID = ++maxId,
                TypeName = "5233"
            };
            Fsql.Insert(invType).ExecuteAffrows();
            InvType invType2 = new InvType()
            {
                TypeID = ++maxId,
                TypeName = "5233"
            };
            InvType invType3 = new InvType()
            {
                TypeID = ++maxId,
                TypeName = "5233"
            };
            List<InvType> invTypes = new List<InvType>()
            {
                invType2, invType3
            };
            Fsql.Insert(invTypes).ExecuteAffrows();
        }

        public static void Update()
        {
            var type = Fsql.Select<InvType>().ToList().Last();
            type.TypeName = "last";
            Fsql.Update<InvType>(type).ExecuteAffrows();
        }
        public static void Delete()
        {
            var type = Fsql.Select<InvType>().ToList().Last();
            Fsql.Delete<InvType>(type).ExecuteAffrows();
        }

        public static void DbFirst()
        {
            //FreeSql.Generator不支持.net core以下版本
        }
    }
}
