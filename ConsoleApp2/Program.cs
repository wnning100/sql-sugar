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
            Class1 db = new Class1(connectionString);
            var result = db.GetStudentList();
            foreach (var item in result)
            {
                Console.WriteLine($"CId:{item.CId} Cname:{item.Cname} TId:{item.TId}");
            }

            //單例 
            var singletonDb = Class1.Instance.GetStudentList2;
            foreach (var item in result)
            {
                Console.WriteLine($"CId:{item.CId} Cname:{item.Cname} TId:{item.TId}");
            }
            // 
            var list = Class1.Instance.GetStudentList2();
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{list[i].SCCId}, {list[i].SCScore}, {list[i].Sname}");
            }
            

            var list2 = Class1.Instance.GetStudentList3();

            for (int i = 0; i < list2.Count; i++)
            {
                Console.WriteLine($"{list2[i].Sname}");
            }

            Console.ReadKey();
        }
    }
}