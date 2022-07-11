using Chloe;
using Chloe.Infrastructure;
using Chloe.PostgreSQL;
using Npgsql;
using ORMTest.WPF.Models.Chloe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Services
{
    public static class ChloeService
    {
        static string ConnString = "PORT=5433;DATABASE=ORMTestChloe;HOST=localhost;PASSWORD=hnddzy;USER ID=hnddzy";
        static PostgreSQLContext DBContext = new PostgreSQLContext(new PostgreSQLConnectionFactory(ConnString))
        {
            ConvertToLowercase = false
        };

        public static void Test()
        {
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
        public static void QueryAll()
        {
            var ls = DBContext.Query<InvType>().ToList();
        }
        public static void QueryWhere()
        {
            var ls = DBContext.Query<InvType>().Where(p => p.GroupID == 6).ToList();
        }
        public static void QuerySelect1()
        {
            var ls2 = from type in DBContext.Query<InvType>()
                      where type.GroupID == 6
                      select type.TypeID;
            var ls = DBContext.Query<InvType>().Where(p => p.GroupID == 6).Select(p=>p.TypeID).ToList();
        }
        public static void QuerySelect2()
        {
            var ls = DBContext.Query<InvType>().Where(p => p.GroupID == 6).Select(p => new {p.TypeID,p.GroupID}).ToList();
        }
        public static void QueryPage()
        {
            var ls = DBContext.Query<InvType>().OrderBy(p => p.TypeID).TakePage(1, 10).ToList();
        }
        public static void QueryComplex1()
        {
            var ls = DBContext.Query<InvType>()
                .InnerJoin<InvGroup>((t, g) => t.GroupID == g.GroupID)
                .Select((t, g) => new { Type = t, Group = g })
                .ToList();
        }
        public static void QueryComplex2()
        {
            var ls = DBContext.Query<InvType>()
                .LeftJoin<InvGroup>((t, g) => t.GroupID == g.GroupID)
                .Select((t, g) => new { Type = t, Group = g })
                .ToList();
        }

        public static void QuerySql()
        {
            var ls = DBContext.SqlQuery<InvType>("SELECT * FROM \"InvTypes\"");
        }
        public static void Insert()
        {
            InvType invType = new InvType();
            invType.TypeID = 5233;
            invType.TypeName = "5233";
            DBContext.Insert(invType);
            //DBContext.Insert<InvType>(() => new InvType()
            //{
            //    TypeID = 5233,
            //    TypeName = "5233"
            //});
            //DBContext.InsertRange();
            //DBContext.BulkInsert
        }

        public static void Update()
        {
            var tpye = DBContext.Query<InvType>().ToList().Last();
            tpye.TypeName = "5230";
            DBContext.Update(tpye);
        }

        public static void Delete()
        {
            var tpye = DBContext.Query<InvType>().ToList().Last();
            DBContext.Delete(tpye);
        }
    }
    public class PostgreSQLConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public PostgreSQLConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection(this._connString);
            return conn;
        }
    }
}
