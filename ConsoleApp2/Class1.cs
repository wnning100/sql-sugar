using ConsoleApp2.Entity;
using ConsoleApp2.Model;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Class1
    {
        //private string? connectionString; // 類別
        private SqlSugarClient db; // 類別
        public static Class1 Instance { get; private set; } // 類別是 Class1
        // 單例
        static Class1() // 靜態類別
        {
            //透過Microsoft.Extensions.Configuration 取得appsettings字串
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                              .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            Instance = new Class1(connectionString); // 賦值給 Instance

        }

        // 多例
        public Class1(string connectionString) // 建構式
        {
            // this.connectionString = connectionString;
            //建立資料庫物件
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,//連線符字串
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
            });

            //新增Sql列印事件，開發中可以刪掉這個程式碼
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);
            };
        }

        // 列出 Course 所有資料
        public List<Course> GetStudentList()
        {
            var list = db.Queryable<Course>().ToList();//查詢表的所有
            return list;
        }
        //  檢索" 01 "課程分數小於 60，按分數降序排列的學生資訊
        public List<Student2> GetStudentList2()
        {
           return db.Queryable<SC>() // step1
            .InnerJoin<Student>((sc, st) => sc.SId == st.SId) // step 2
            .Where(sc => sc.Score < 60 && sc.CId == "01") // step3
            .Select((sc, st) => new Student2 { SCCId = sc.CId, SCScore = sc.Score, Sname = st.Sname }) // step4
            .MergeTable().OrderBy(x => x.SCScore, OrderByType.Desc) // step5
            .ToList();
        }

        // practice Student id = 01 的人
        internal List<Student> GetStudentList3()
        {
            return db.Queryable<Student>().Where(it => it.SId == "01").ToList();
        }

        // 沒用到
        //public static SqlSugarClient GetInstance()
        //{
        //    //建立資料庫物件
        //    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        //    {
        //        ConnectionString = "Server=MSI\\SQLEXPRESS01;database=Practice;Uid=Yang;Password=1234qwerasdf",//連線符字串
        //        DbType = DbType.SqlServer,
        //        IsAutoCloseConnection = true,
        //    });

        //    //新增Sql列印事件，開發中可以刪掉這個程式碼
        //    db.Aop.OnLogExecuting = (sql, pars) =>
        //    {
        //        Console.WriteLine(sql);
        //    };
        //    return db;
        //}
    }
}
