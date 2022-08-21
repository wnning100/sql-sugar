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
            for (int i = 0; i < n2.Count; i++) {
                Console.WriteLine($"學號:{n2[i].StSId},平均成績:{n2[i].AvgScore}");
            }
            // 3 
            // var n3 = Class1.Instance.Sql3();
            

            // 4
            var countTeacher = Class1.Instance.Sql4();
            Console.WriteLine($"{countTeacher}");

            // 6
            var n6 = Class1.Instance.Sql6();
            for (int i = 0; i < n6.Count; i++)
            {
                Console.WriteLine($"學號:{n6[i].StudentId},姓名:{n6[i].StudentName}");
            }
            // 7 
            //var n7 = Class1.Instance.Sql7();
            //for (int i = 0; i < n7.Count; i++)
            //{
            //    Console.WriteLine($"學號:{n7[i].StudentId},姓名:{n7[i].StudentName}");
            //}

            // 8
            var n8 = Class1.Instance.Sql8();
            Console.WriteLine($"總成績:{n8}");

            // 9
            var n9 = Class1.Instance.Sql9();
            for (int i = 0; i < n9.Count; i++)
            {
                Console.WriteLine($"學號:{n9[i].StudentId},姓名:{n9[i].StudentName}");
            }

            // 15
            var n15 = Class1.Instance.Sql15();
            for (int i = 0; i < n15.Count; i++)
            {
                Console.WriteLine($"學號:{n15[i].StSId},姓名:{n15[i].StudentName},平均成績:{n15[i].AvgScore}");
            }

            // 16
            var n16 = Class1.Instance.Sql16();
            for (int i = 0; i < n16.Count; i++)
            {
                Console.WriteLine($"學號:{n16[i].StudentId},姓名:{n16[i].StudentName},性別:{n16[i].StudentSex},生日:{n16[i].StudentAge}");           
            }


            // 17
            //var n17 = Class1.Instance.Sql17();

            

            

            Console.ReadKey();
        }
    }
}