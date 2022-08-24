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
            // 5
            var n5 = Class1.Instance.Sql5();
            Console.WriteLine(5);
            foreach (var item in n5)
            {
                Console.WriteLine($"學號:{item.StudentId},姓名:{item.StudentName}");
            }
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
            // 10

            // 11

            // 12

            // 13

            // 14

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
            // 18
            var n18 = Class1.Instance.Sql18();
            Console.WriteLine(18);
            foreach (var item in n18)
            {
                Console.WriteLine($"課程名稱:{item.CourseName},最高分:{item.MaxScore},最低分:{item.MinScore},平均分數:{item.AvgScore}");
            }
            // 19
            var n19 = Class1.Instance.Sql19();
            Console.WriteLine(19);
            foreach (var item in n19)
            {
                Console.WriteLine($"學號:{item.SId},姓名:{item.Sname},總分:{item.SumScore},排名:{item.Rank}");
            }
            // 20
            var n20 = Class1.Instance.Sql20();
            Console.WriteLine(20);
            foreach (var item in n20)
            {
                Console.WriteLine($"老師:{item.TeacherName},課號:{item.CourseId},平均分數:{item.AvgScore}");
            }
            // 21
            var n21 = Class1.Instance.Sql21();
            Console.WriteLine(21);
            foreach (var item in n21)
            {
                Console.WriteLine($"學號:{item.SId},姓名:{item.Sname},平均分數:{item.AvgScore},排名:{item.Rank}");
            }
            // 26
            var n26 = Class1.Instance.Sql26();
            Console.WriteLine(26);
            foreach (var item in n26)
            {
                Console.WriteLine($"課號:{item.CourseID},課程名稱:{item.CourseName},[100-85]:{item.Case100To85},[85-70]:{item.Case85To70},[70-60]:{item.Case70To60},[<60]:{item.CaseFail}");
            }
            // 27
            var n27 = Class1.Instance.Sql27();
            Console.WriteLine(27);
            foreach (var item in n27)
            {
                Console.WriteLine($"課號:{item.CId},選修人數:{item.StudentNumber}");
            }
            // 28
            var n28 = Class1.Instance.Sql28();
            Console.WriteLine(28);
            foreach (var item in n28)
            {
                Console.WriteLine($"學號:{item.StudentId},姓名:{item.StudentName}");
            }
            // 29
            var n29 = Class1.Instance.Sql29();
            Console.WriteLine(29);
            foreach (var item in n29)
            {
                Console.WriteLine($"性別:{item.Sex},人數:{item.GenderAmount}");
            }
            // 30
            var n30 = Class1.Instance.Sql30();
            Console.WriteLine(30);
            foreach (var item in n30)
            {
                Console.WriteLine($"{item.Sname},{item.Ssex},{item.Sage}");
            }
            // 31
            //var n31 = Class1.Instance.Sql31();
            //Console.WriteLine(31);
            //foreach (var item in n31)
            //{
            //    Console.WriteLine($"1990出生的學生名單:{item.Sname},{item.Ssex},{item.Sage}");
            //}
            // 33
            var n33 = Class1.Instance.Sql33();
            Console.WriteLine(33);
            foreach (var item in n33)
            {
                Console.WriteLine($"{item.CId},{item.AvgScore}");
            }
            // 34
            var n34 = Class1.Instance.Sql34();
            Console.WriteLine(34);
            foreach (var item in n34)
            {
                Console.WriteLine($"姓名:{item.Sname},課程名稱:{item.Cname},分數:{item.Score}");
            }
            // 35
            var n35 = Class1.Instance.Sql35();
            Console.WriteLine(35);
            foreach (var item in n35)
            {
                Console.WriteLine($"姓名:{item.Sname},課程名稱:{item.Cname},分數:{item.Score}");
            }
            // 36
            var n36 = Class1.Instance.Sql36();
            Console.WriteLine(36);
            foreach (var item in n36)
            {
                Console.WriteLine($"姓名:{item.Sname},課程名稱:{item.Cname},分數:{item.Score}");
            }
            // 37
            var n37 = Class1.Instance.Sql37();
            Console.WriteLine(37);
            foreach (var item in n37)
            {
                Console.WriteLine($"課號:{item.CID}");
            }
            // 38
            var n38 = Class1.Instance.Sql38();
            Console.WriteLine(38);
            foreach (var item in n38)
            {
                Console.WriteLine($"學號:{item.StudentId},姓名:{item.StudentName}");
            }
            // 39
            var n39 = Class1.Instance.Sql39();
            Console.WriteLine(39);
            foreach (var item in n39)
            {
                Console.WriteLine($"課號:{item.CId},選修人數:{item.CountAmount}");
            }
            // 分组查询和使用
            // 42
            var n42 = Class1.Instance.Sql42();
            Console.WriteLine(42);
            foreach (var item in n42)
            {
                Console.WriteLine($"課號:{item.CId},選修人數:{item.CountAmount}");
            }
            // 43
            var n43 = Class1.Instance.Sql43();
            Console.WriteLine(43);
            foreach (var item in n43)
            {
                Console.WriteLine($"學號:{item.SID}");
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

            
            Console.ReadKey();

        }
    }
}