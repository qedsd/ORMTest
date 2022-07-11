using Microsoft.EntityFrameworkCore;
using ORMTest.WPF.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Services
{
    public static class EFService
    {
        public static void Test()
        {
            QueryAll();
            QueryWhere();
            QuerySelect1();
            QuerySelect2();
            QueryPage();
            QuerySql();
            QueryComplex1();
            QueryComplex2();
            Insert();
            Update();
            Delete();
        }
        public static void QueryAll()
        {
            using(var db = new TypeContext())
            {
                var ls = db.InvTypes.ToList();
            }
        }
        public static void QueryWhere()
        {
            using (var db = new TypeContext())
            {
                var ls = db.InvTypes.Where(p => p.GroupID == 6).ToList();
            }
        }
        public static void QuerySelect1()
        {
            using (var db = new TypeContext())
            {
                var ls2 = (from type in db.InvTypes
                           where type.GroupID > 2
                           select type.TypeID).ToList();
                var ls = db.InvTypes.Where(p => p.GroupID == 6).Select(p=>p.TypeID).ToList();
            }
        }
        public static void QuerySelect2()
        {
            using (var db = new TypeContext())
            {
                var ls = db.InvTypes.Where(p => p.GroupID == 6).Select(p => new { p.TypeID,p.GroupID}).ToList();
            }
        }

        public static void QueryPage()
        {
            int lastId = 2;
            using (var db = new TypeContext())
            {
                //var ls = db.InvTypes.Skip(2).Take(10);//官方不建议，skip的条目依然会查询一遍
                var ls = db.InvTypes.Where(p => p.TypeID > lastId).Take(10).ToList();//记录上一次的id来手动分页
            }
        }

        public static void QuerySql()
        {
            using (var db = new TypeContext())
            {
                var ls = db.InvTypes.FromSqlRaw("SELECT * FROM \"InvTypes\"");
            }
        }

        public static void QueryComplex1()
        {
            using (var db = new TypeContext())
            {
                var q = from t in db.InvTypes
                        from g in db.InvGroups
                        select new { t, g };
            }
        }
        public static void QueryComplex2()
        {
            using (var db = new TypeContext())
            {
                var q = from t in db.InvTypes
                        join g in db.InvGroups
                        on t.GroupID equals g.GroupID into grouping
                        from p in grouping.DefaultIfEmpty()
                        select new { t, p };
            }
        }

        public static void Insert()
        {
            using (var db = new TypeContext())
            {
                InvType invType = new InvType()
                {
                    TypeID = 5233,
                    TypeName = "5233"
                };
                db.InvTypes.Add(invType);
                db.SaveChanges();
            }
        }

        public static void Update()
        {
            using (var db = new TypeContext())
            {
                var type = db.InvTypes.ToList().LastOrDefault();
                type.TypeName = "5230";
                db.SaveChanges();
            }
        }

        public static void Delete()
        {
            using (var db = new TypeContext())
            {
                var type = db.InvTypes.ToList().LastOrDefault();
                db.Remove(type);
                db.SaveChanges();
            }
        }
    }

    public class TypeContext:DbContext
    {
        public DbSet<InvType> InvTypes { get; set; }
        public DbSet<InvGroup> InvGroups { get; set; }
        //Npgsql.EntityFrameworkCore.PostgreSQL
        protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseNpgsql("PORT=5433;DATABASE=ORMTestEF;HOST=localhost;PASSWORD=hnddzy;USER ID=hnddzy");

    }
}
