using ConsoleApp2;
using ConsoleApp2.Model;
using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace Console_ConfigTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // 透過 Microsoft.Extensions.Configuration 取得 appsettings 字串
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                              .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            //多例
            //Class1 db = new Class1(connectionString);
            //var result = db.GetStudentList();
            //foreach (var item in result)
            //{
            //    Console.WriteLine($"CId:{item.CId} Cname:{item.Cname} TId:{item.TId}");
            //}

            //單例 
            // 1
            //var n1 = Class1.Instance.Sql1();
            //Console.WriteLine(n1.Count);
            //for (int i = 0; i < n1.Count; i++)
            //{
            //    Console.WriteLine($"學號:{n1[i].SID}");
            //}

            // 2
            var n2 = Class1.Instance.Sql2();
            Console.WriteLine(2);
            for (int i = 0; i < n2.Count; i++) {
                Console.WriteLine($"學號:{n2[i].StSId},平均成績:{n2[i].AvgScore}");
            }
            // 3 
            // var n3 = Class1.Instance.Sql3();


            // 4
            var countTeacher = Class1.Instance.Sql4();
            Console.WriteLine(4);
            Console.WriteLine($"{countTeacher}");

            // 6
            var n6 = Class1.Instance.Sql6();
            Console.WriteLine(6);
            for (int i = 0; i < n6.Count; i++)
            {
                Console.WriteLine($"學號:{n6[i].StudentId},姓名:{n6[i].StudentName}");
            }
            // 7 
            //var n7 = Class1.Instance.Sql7();
            //Console.WriteLine(7);
            //for (int i = 0; i < n7.Count; i++)
            //{
            //    Console.WriteLine($"學號:{n7[i].StudentId},姓名:{n7[i].StudentName}");
            //}

            // 8
            var n8 = Class1.Instance.Sql8();
            Console.WriteLine(8);
            Console.WriteLine($"總成績:{n8}");

            // 9
            var n9 = Class1.Instance.Sql9();
            Console.WriteLine(9);
            for (int i = 0; i < n9.Count; i++)
            {
                Console.WriteLine($"學號:{n9[i].StudentId},姓名:{n9[i].StudentName}");
            }

            // 15
            var n15 = Class1.Instance.Sql15();
            Console.WriteLine(15);
            for (int i = 0; i < n15.Count; i++)
            {
                Console.WriteLine($"學號:{n15[i].StSId},姓名:{n15[i].StudentName},平均成績:{n15[i].AvgScore}");
            }

            // 16
            var n16 = Class1.Instance.Sql16();
            Console.WriteLine(16);
            for (int i = 0; i < n16.Count; i++)
            {
                Console.WriteLine($"學號:{n16[i].StudentId},姓名:{n16[i].StudentName},性別:{n16[i].StudentSex},生日:{n16[i].StudentAge}");
            }


            // 17
            //var n17 = Class1.Instance.Sql17();

            var n26 = Class1.Instance.Sql26();
            Console.WriteLine(26);
            foreach (var item in n26)
            {
                Console.WriteLine($"課號:{item.CourseID},課程名稱:{item.CourseName},[100-85]:{item.Case100To85},[85-70]:{item.Case85To70},[70-60]:{item.Case70To60},[<60]:{item.CaseFail}");
            }
            
            // Doc
            // **基本查询**
            // 查詢表的所有
            var allCourse = Class1.Instance.GetCourseList();
            for (int i = 0; i < allCourse.Count; i++) {
                Console.WriteLine($"{allCourse[i].CId},{allCourse[i].TId},{allCourse[i].Cname}");
            }
            // 模糊查詢
            var mm = Class1.Instance.GetName();
            for (int i = 0; i < mm.Count; i++)
            {
                Console.WriteLine(mm[i].Sname);
            }

            // 动态OR查询

            // 查前几条
            var top5 = Class1.Instance.Get5();
            for (int i = 0; i < top5.Count; i++)
            {
                Console.WriteLine($"{top5[i].SId},{top5[i].Sname}");
            }

            // 查询最大值
            var maxScore = Class1.Instance.GetMax();
            Console.WriteLine($"MaxScore:{maxScore}");
            // 查詢第一條
            var first = Class1.Instance.GetFirst();
            Console.WriteLine(first.Name);

            // 根據主鍵查詢
            var pk = Class1.Instance.GetPk();
            Console.WriteLine(pk.Name);
            // 多條件查詢
            var many = Class1.Instance.GetMany();
            for (int i = 0; i < many.Count; i++)
            {
                Console.WriteLine($"{many[i].Name}");
            }
            // 查詢是否存在记录
            var exist = Class1.Instance.IfExist();
            Console.WriteLine(exist);

            // 數據行數
            var amount = Class1.Instance.GetCount();
            Console.WriteLine(amount);
            // 求和
            var sumScore = Class1.Instance.GetSum();
            Console.WriteLine(sumScore);

            // **分頁查詢**
            // 單表分頁
            var singlePage = Class1.Instance.GetSinglePage();
            // Console.WriteLine(singlePage);
            for (int i = 0; i < singlePage.Count; i++)
            {
                Console.WriteLine(singlePage[i].Sname);
            }
            // 多表分頁
            var manyPages = Class1.Instance.GetManyTablesPage();
            for (int i = 0; i < manyPages.Count; i++)
            {
                Console.WriteLine(manyPages[i].Sname);
            }
            // 排序
            var orderBy = Class1.Instance.GetOrders();
            for (int i = 0; i < orderBy.Count; i++)
            {
                Console.WriteLine($"{orderBy[i].SId},{orderBy[i].Sname}");
            }
            // 隨機取5條排序
            var orderRandom = Class1.Instance.Getrandom();
            for (int i = 0; i < orderRandom.Count; i++)
            {
                Console.WriteLine($"{orderRandom[i].SId},{orderRandom[i].Sname}");
            }

            // 分组查询和使用
            var n42 = Class1.Instance.Sql42();
            foreach (var item in n42)
            {
                Console.WriteLine($"課號:{item.CId},選修人數:{item.CountAmount}");
            }

            var n43 = Class1.Instance.Sql43();
            foreach (var item in n43)
            {
                Console.WriteLine($"學號:{item.SID}");
            }
            Console.ReadKey();

        }
    }
}