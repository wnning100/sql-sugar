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

        // 1.查詢課程編號為「01」的課程比「02」的課程成績高的所有學生的學號（難）


        // 2. 查詢平均成績大於60分的學生的學號和平均成績
        public List<StIdAvgScore> Sql2()
        {
            return db.Queryable<SC>()
                .GroupBy(sc => sc.SId)
                .Having(sc => SqlFunc.AggregateAvg(sc.Score) > 60)
                .Select(sc => new StIdAvgScore { StSId = sc.SId, AvgScore = SqlFunc.AggregateAvg(sc.Score), })
                .ToList();
        }
        // 3.查詢所有學生的學號、姓名、選課數、總成績
        // select St.sid '學號', St.sname '姓名', countCid '選課數', sumScore '總成績'
        //from Student St
        //left join
        //(select SId, count(CId) countCid, sum(score) sumScore
        //from sc group by sid )sc
        //on St.SId = sc.sid;


        //public List<StCourse> Sql3()
        //{
        //    var list = db.Queryable<SC>()
        //                .GroupBy(sc => sc.SId)
        //                .Select(sc => new
        //                {
        //                    ScSId = sc.SId,
        //                    Cnt = SqlFunc.AggregateCount(sc),
        //                    SumScore = SqlFunc.AggregateSum(sc.Score),
        //                });
        //    return list;
        //    var list2 = db.Queryable<Student>()
        //                .LeftJoin(list, (st, sc) => sc.ScSId == st.SId)
        //                .Select((st, sc) => new StCourse
        //                {
        //                    StudentName = st.Sname,
        //                    StudentSId = sc.ScSId,
        //                    CountCourse = sc.Cnt,
        //                    SumScore = sc.SumScore,
        //                })
        //                .ToList();
        //    return list2;

        //}


        // 4. 查詢姓「猴」的老師的個數
        // 回傳的結果是個數，型別是 int
        public int Sql4()
        {
            return db.Queryable<Teacher>()
                .Where(t => t.Tname.Contains('猴'))
                .Count();
        }
        // 5.查詢沒學過「張三」老師課的學生的學號、姓名
        /*
         SELECT St.Sname, St.SId FROM Student St WHERE St.SId NOT IN(
         SELECT Sc.SId FROM  SC Sc
         JOIN Course c ON c.CId = Sc.CId
         JOIN Teacher t ON t.TId = c.TId
         WHERE t.Tname = '張三'
        );
         */
        //public List<SIdSname> Sql5()
        //{
        //    return db.Queryable<Student>()
        //        .Where(st => !st.SId.Contains(
        //                db.Queryable<SC>()
        //                .InnerJoin<Course>((c, sc) => c.CId == sc.CId)
        //                .InnerJoin<Teacher>((c, sc, t) => t.TId == sc.TId))

        //             TName = 張三還沒寫進來
        //            ))
        //        .Select(st => new SIdSname { StudentName = st.Sname, StudentId = st.SId })
        //        .ToList();
        //}


        // 6. 查詢學過「張三」老師所教的所有課的同學的學號、姓名
        //select St.SId, St.Sname from Student St, SC Sc, Course C, Teacher T
        //where St.SId = Sc.SId and Sc.CId = C.CId and C.TId = T.TId and T.Tname = '張三';
        public List<SIdSname> Sql6()
        {
            return db.Queryable<SC, Student, Course, Teacher>((sc, st, c, t) => sc.CId == c.CId && c.TId == t.TId && st.SId == sc.SId)
                .Where((sc, st, c, t) => t.Tname == "張三")
                .Select((sc, st, c, t) => new SIdSname { StudentId = sc.SId, StudentName = st.Sname })
                .ToList();
        }

        // 7. 查詢學過編號為「01」的課程並且也學過編號為「02」的課程的學生的學號、姓名
        // 不要自己join自己，可以用 where or and 等條件處理完再join
        public List<SIdSname> Sql7()
        {
            return db.Queryable(db.Queryable<SC>().Where(sc => sc.CId == "01"),
                             db.Queryable<SC>().Where(sc => sc.CId == "02"),
                             (sc1, sc2) => sc1.SId == sc2.SId)
                 .Where((sc1, sc2) => sc1.SId == sc2.SId)
                 .Select((sc1, sc2) => new SIdScore { SId = sc1.CId, Score = sc1.Score })
                 .RightJoin<Student>((sc1, st) => sc1.SId == st.SId)
                 .Select((sc1, st) => new SIdSname { StudentId = sc1.SId, StudentName = st.Sname })
                 .ToList();
        }

        // 8. 查詢課程編號為「02」的總成績
        public decimal Sql8()
        {
            return db.Queryable<SC>()
                .Where(sc => sc.CId == "02")
                .Sum(sc => sc.Score);
        }

        // 9.查詢課程成績小於60分的學生的學號、姓名，不能重複
        //SELECT DISTINCT St.SId, St.Sname FROM Student St
        //JOIN(SELECT DISTINCT Sc.SId from SC WHERE Sc.score< 60) Sc
        //ON St.SId = Sc.SId


        //var query_9 = DB
        //                .Queryable<SC, Student>((sc, st) => sc.SId == st.SId)
        //                .Where((sc, st) => sc.Score < 60)
        //                .Select((sc, st) => new { sc.SId, st.SName, sc.Score })
        //                .ToList();

        //    for (int i = 0; i<query_9.Count; i++)
        //    {
        //        Console.WriteLine($"query_9 SId= {query_9[i].SId}, {query_9[i].SName}, {query_9[i].Score}");
        //    }

        //public List<SIdSname> Sql9()
        //{
        //   return  db.Queryable(db.Queryable<Student>(),
        //                          db.Queryable<SC>().Select<SIdScore>(sc => new SIdScore { SId = sc.SId, Score = sc.Score }),
        //                          JoinType.Inner,
        //                          (st, sc) => st.SId == sc.SId)
        //               .Where((st, sc) => sc.Score < 60)
        //               .Distinct()
        //               .Select((st, sc) => new SIdSname { StudentId = st.SId, StudentName = st.Sname })
        //               .ToList();
        //}

        public List<SIdSname> Sql9() {
            return db.Queryable<Student>()
                       .InnerJoin(db.Queryable<SC>().Select<SIdScore>(sc => new SIdScore { SId = sc.SId, Score = sc.Score })
                                  , (st, sidscore) => st.SId == sidscore.SId)
                       .Where((st, sidscore) => sidscore.Score < 60)
                       .Distinct()
                       .Select((st, sidscore) => new SIdSname { StudentId = st.SId, StudentName = st.Sname })
                       .ToList();
        }
        // 10.查詢沒有學全所有課的學生的學號、姓名
        // 11.查詢至少有一門課與學號為「01」的學生所學課程相同的學生的學號和姓名 （難）
        // 12.查詢和「01」號同學所學課程完全相同的其他同學的學號（難）

        // 15.查詢兩門及其以上不及格課程的同學的學號，姓名及其平均成績
        //public List<> Sq15()
        //{ 
        //    return db.Queryable<SC, Student>((sc, st) => sc.SId == st.SId)
        //      .Where((sc, st) => sc.Score < 60)
        //      .GroupBy((sc, st) => new { sc.SId, st.Sname })
        //      .Having((sc, st) => SqlFunc.AggregateCount(sc.CId) >= 2)
        //      .Select((sc, st) => new { sc.SId, st.Sname, avgScore = SqlFunc.AggregateAvg(sc.Score) })
        //      .ToList();
        //}

        //select Sc.SId, St.Sname, Sc.sccc avgScore from
        //(select SId, AVG(score) sccc from SC group by SId having avg(score) < 60 and count(CId) >= 2) Sc,
        //(select Sname, SId from Student group by SId, Sname) St
        //WHERE St.SId = Sc.SId
        public List<StudentSC> Sql15()
        {
            return db.Queryable(
                     db.Queryable<SC>()
                       .GroupBy(sc => sc.SId)
                       .Having(sc => SqlFunc.AggregateAvg(sc.Score) < 60
                                     && SqlFunc.AggregateCount(sc.CId) >= 2)
                       .Select(sc => new StIdAvgScore { StSId = sc.SId, AvgScore = SqlFunc.AggregateAvg(sc.Score) }),
                     db.Queryable<Student>()
                       .GroupBy(st => new { st.SId, st.Sname })
                       .Select(st => new SIdSname { StudentId = st.SId, StudentName = st.Sname }),
                         (sc, st) => sc.StSId == st.StudentId
                     )
                     .Select((sc, st) => new StudentSC { StSId = sc.StSId, StudentName = st.StudentName, AvgScore = sc.AvgScore })
                     .ToList();
        }


        // 16、檢索"01"課程分數小於60，按分數降序排列的學生資訊
        //select Sc.CId, Sc.score, Student.* from SC as Sc
        //join Student on Student.SId = Sc.SId
        //where Sc.CId = '01' and Sc.score< 60
        //order by Sc.score desc

        public List<StudentDetail> Sql16()
        {
            return db.Queryable<SC, Student>((sc, st) => sc.SId == st.SId && sc.CId == "01" && sc.Score < 60)
                .OrderBy((sc, st) => sc.Score, OrderByType.Desc)
                .Select((sc, st) => new StudentDetail { StudentId = st.SId, StudentName = st.Sname, StudentSex = st.Ssex, StudentAge = st.Sage })
                .ToList();
        }

        // 17. 按平均成績從高到低顯示所有學生的所有課程的成績以及平均成績(難)
        /*
        select Sc.SId, Sc.CId, Sc.score, AvgScore.average from SC
        join
        (select AVG(score) 'average', Sc.SId from SC as Sc
        join Course on Sc.CId = Course.CId
        group by Sc.SId) as AvgScore
        on SC.SId = AvgScore.SId
        order by AvgScore.average desc
        */

        //public List<> Sql17()
        //{

        //}
        // 18.查詢各科成績最高分、最低分和平均分
        /*
         select MAX(score) '最高分',MIN(score) '最低分',AVG(score) '平均分', Course.Cname '課程名稱' from SC as Sc
            join Course on Sc.CId = Course.CId
            group by Sc.CId, Course.Cname
         */

        // result count = 3 but n18 is null, I don't know whyyyy
        public List<ScoreData> Sql18()
        {
            var result = db.Queryable<SC>()
                .InnerJoin<Course>((sc, c) => sc.CId == c.CId)
                .GroupBy((sc, c) => new { sc.CId, c.Cname})
                .Select((sc, c) => new ScoreData { CourseName = c.Cname, MaxScore = SqlFunc.AggregateMax(sc.Score), MinScore = SqlFunc.AggregateMin(sc.Score), AvgScore = SqlFunc.AggregateAvg(sc.Score) })
                .ToList();
            return result;
        }
        // 19.查詢學生的總成績並進行排名
        /*
         select Sc.SId, Stc.Sname, sum(sc.score) as sumScore, rank() over (order by sum(sc.score) DESC) as No from SC Sc
            join (select St.SId,St.Sname from Student St) Stc
            on Stc.SId = Sc.SId
            group by Sc.SId, Stc.Sname
            order by sumScore desc
         */

        // 20.查詢不同老師所教不同課程平均分,從高到低顯示
        public List<TeacherCourse> Sql20()
        { 
            return db.Queryable<SC, Teacher, Course>((sc, t, c) => sc.CId == c.CId && c.TId == t.TId)
                             .GroupBy((sc, t, c) => new { sc.CId, t.Tname, c.Cname })
                             .Select((sc, t, c) => new TeacherCourse { CourseId = sc.CId, TeacherName = t.Tname, AvgScore = SqlFunc.AggregateAvg(sc.Score) })
                             .MergeTable().OrderBy(x => x.AvgScore)
                             .ToList();
        }
        // 21.查詢學生平均成績及其名次

        // 22.按各科成績進行排序，並顯示排名(難)

        // 23.查詢每門功課成績最好的前兩名學生姓名

        // 24.查詢所有課程的成績第2名到第3名的學生資訊及該課程成績

        // 25.查詢各科成績前三名的記錄（不考慮成績並列情況）

        // 26.使用分段[100-85],[85-70],[70-60],[<60]來統計各科成績，分別統計各分數段人數：課程ID和課程名稱
        /*
        SELECT Sc.CId, c.Cname,
        SUM(CASE WHEN Sc.score< 100 AND Sc.score >= 85 THEN 1 ELSE 0 END) AS "100-85",
        SUM(CASE WHEN Sc.score< 85 AND Sc.score >= 70 THEN 1 ELSE 0 END) AS "85-70",
        SUM(CASE WHEN Sc.score< 70 AND Sc.score >= 60 THEN 1 ELSE 0 END) AS "70-60",
        SUM(CASE WHEN Sc.score< 60 THEN 1 ELSE 0 END) AS "<60"
        FROM SC Sc JOIN Course C ON C.CId = Sc.CId
        GROUP BY Sc.CId, C.Cname;
        */
        // case when sugar用法
        //SqlFunc.IF(st.Id > 1)
        // .Return(st.Id)
        // .ElseIF(st.Id == 1)
        // .Return(st.SchoolId).End(st.Id)

        public List<CourseStatistics> Sql26()
        {
            return db.Queryable<SC, Course>((sc, c) => sc.CId == c.CId)
                .GroupBy((sc, c) => new { sc.CId, c.Cname })
                .Select((sc, c) => new CourseStatistics
                {
                    CourseID = sc.CId,
                    CourseName = c.Cname,
                    Case100To85 = SqlFunc.AggregateSum(SqlFunc.IF(sc.Score < 100 && sc.Score >= 85).Return(1).End(0)),
                    Case85To70 = SqlFunc.AggregateSum(SqlFunc.IF(sc.Score < 85 && sc.Score >= 70).Return(1).End(0)),
                    Case70To60 = SqlFunc.AggregateSum(SqlFunc.IF(sc.Score < 70 && sc.Score >= 60).Return(1).End(0)),
                    CaseFail = SqlFunc.AggregateSum(SqlFunc.IF(sc.Score < 60).Return(1).End(0))
                })
                .ToList();

        }
        // 27.查詢每門課程被選修的學生數
        // 28.查詢出只有兩門課程的全部學生的學號和姓名
        /*
         select St.SId, St.Sname from Student St
            join (select SId, COUNT(CId) countCid from SC group by SId) Sc
            on St.SId = Sc.SId
            where Sc.countCid = 2
            group by St.SId, St.Sname
         */
        //public List<SIdSname> Sql28()
        //{ 
        //    return db.Queryable<Student, SC>((st, sc) => st.SId == sc.SId)
        //        .Select(st => new SIdSname { StudentId = st.SId, StudentName = st.Sname })
        //        .Where(sc => sc.)
        //}
        // 29.查詢男生、女生人數
        /*
         select Ssex '性別', count(Ssex) '人數' from Student St
           group by Ssex
         */
        public List<GenderCount> Sql29()
        {
            return db.Queryable<Student>()
                .GroupBy(st => st.Ssex)
                .Select(st => new GenderCount { Sex = st.Ssex, GenderAmount = SqlFunc.AggregateCount(st.Ssex) })
                .ToList();
        }
        // 30.查詢名字中含有"風"字的學生資訊
        public List<Student> Sql30()
        {
            return db.Queryable<Student>()
                .Where(st => st.Sname.Contains('風'))
                .ToList();
        }
        // 31.查詢1990年出生的學生名單
        // 32.查詢平均成績大於等於85的所有學生的學號、姓名和平均成績
        // 33.查詢每門課程的平均成績，結果按平均成績升序排序，平均成績相同時，按課程號降序排列
        /*
         select CId,AVG(score) as avgScore from SC
        group by CId
        order by avgScore ASC, CId DESC;
         */
        public List<CourseAvgScore> Sql33()
        {
            return db.Queryable<SC>()
                .GroupBy(sc => sc.CId)
                .Select(sc => new CourseAvgScore { CId = sc.CId, AvgScore = SqlFunc.AggregateAvg(sc.Score) })
                .OrderBy(sc => new { sc.AvgScore, OrderByType.Asc ,sc.CId, OrderByType.Desc})
                .ToList();
                
        }
        // 34.查詢課程名稱為"數學"，且分數低於60的學生姓名和分數
        // 35.查詢所有學生的課程及分數情況
        // 36.查詢任何一門課程成績在70分以上的姓名、課程名稱和分數
        // 37.查詢不及格的課程並按課程號從大到小排列
        // 38.查詢課程編號為03且課程成績在80分以上的學生的學號和姓名
        // 39.求每門課程的學生人數
        // 40.查詢選修「張三」老師所授課程的學生中成績最高的學生姓名及其成績
        // 41.查詢不同課程成績相同的學生的學生編號、課程編號、學生成績 （難）

        // 分组查询和使用
        // 統計每門課程的學生選修人數（超過5人的課程才統計）。要求輸出課程號和選修人數，查詢結果按人數降序排列，若人數相同，按課程號升序排列
        public List<CountCourse> Sql42()
        {
            return db.Queryable<SC>()
                .GroupBy(sc => sc.CId)
                .Having(sc => SqlFunc.AggregateCount(sc.CId) > 5)
                .OrderBy(sc => SqlFunc.AggregateCount(sc.CId), OrderByType.Desc)
                .Select(sc => new CountCourse { CId = sc.CId, CountAmount = SqlFunc.AggregateCount(sc.CId) })
                .ToList();
        }
        // 至少選修兩門課程的學生學號
        public List<StudentID> Sql43()
        {
            return db.Queryable<SC>()
                .GroupBy(sc => new { sc.SId })
                .Having(sc => SqlFunc.AggregateCount(sc.CId) >= 2)
                .Select(sc => new StudentID { SID = sc.SId })
                .ToList();

        }

        // 44、查詢選修了全部課程的學生資訊
        // 45、查詢各學生的年齡
        // 49、查詢本月過生日的學生
        // 50、查詢下一個月過生日的學生








        // Doc
        // **基本查询**
        // 查詢表的所有
        public List<Course> GetCourseList()
        {
            return db.Queryable<Course>().ToList();
        }
        // 模糊查詢
        public List<Student> GetName()
        {
            return db.Queryable<Student>().Where(it => it.Sname.Contains("四")).ToList();

        }
        // 查詢前幾條
        public List<Student> Get5()
        {
            return db.Queryable<Student>().Take(5).ToList();
        }
        // 查詢最大值
        public decimal GetMax()
        {
            return db.Queryable<SC>().Max(it => it.Score);
        }
        // 查詢第一條
        public StudentData GetFirst()
        {
            return db.Queryable<StudentData>()
                .First(it => it.ID == 1);
        }

        // 根据主键查询
        public StudentData GetPk()
        {
            // return db.Queryable<StudentData>().Single(it => it.ID == 2);
            return db.Queryable<StudentData>().InSingle(2); // Model要設主鍵
        }
        // 多條件查詢
        public List<StudentData> GetMany()
        {
            return db.Queryable<StudentData>().Where(it => it.ID < 10 && it.Name.Contains("明")).ToList();
        }
        // 查詢是否存在记录
        public bool IfExist()
        {
            return db.Queryable<StudentData>().Where(it => it.ID > 11).Any();
        }
        // 数据行数
        public int GetCount()
        {
            return db.Queryable<StudentData>().Where(it => it.ID < 10).Count();
        }
        // 求和
        public decimal GetSum()
        {
            return db.Queryable<SC>().Sum(it => it.Score);
        }

        // 查詢最後一條
        // In查询,IN的使用
        // 动态OR查询
        // 查询过滤某一个字段

        // **分頁查詢**
        int pageIndex = 1; // pageindex是从1开始的不是从零开始的
        // 若pageIndex = 2 則顯示第二頁的結果
        int pageSize = 5; // 表示一頁顯示幾筆資料
        int totalCount = 0;
        //单表分页
        public List<Student> GetSinglePage()
        {
            return db.Queryable<Student>().ToPageList(pageIndex, pageSize, ref totalCount);
        }
        // 多表分頁
        public List<Student> GetManyTablesPage()
        {
            return db.Queryable<Student>().LeftJoin<SC>((st, sc) => st.SId == sc.SId)
                .Distinct().Select((st, sc) => new Student { SId = st.SId, Sname = st.Sname, Sage = st.Sage, Ssex = st.Ssex })
                .ToPageList(pageIndex, pageSize, ref totalCount);
        }
        // 如何获取 row_index 的值

        // ** 排序 OrderBy **
        // 表達式排序
        public List<Student> GetOrders()
        {
            return db.Queryable<Student>().OrderBy(it => it.Sname).OrderBy(it => it.SId, OrderByType.Desc).ToList(); // 名字一樣的時候id大到小排序
        }
        // 动态排序
        // 随机排序取5条資料
        public List<Student> Getrandom()
        {
            return db.Queryable<Student>().OrderBy(st => SqlFunc.GetRandom()).Take(5).ToList();
        }
        // OrderByIF

        // ** 动态表达式 **

        // ** 分组查询、重复、去重 **
        
        //select St.sid '學號', St.sname '姓名', countCid '選課數', sumScore '總成績'
        //from Student St
        //left join
        //(select SId, count(CId) countCid, sum(score) sumScore
        //from sc group by sid )sc
        //on St.SId = sc.sid;


        //public List<Student> GetCourse()
        //{ 
        //    return db.Queryable<Student>()
        //        .LeftJoin(db.Queryable<SC>().Select(sc => sc.SId, SqlFunc.AggregateCount(sc.CId), SqlFunc.AggregateSum(sc.score)))
        //}

        // 特殊日期分组
        // Count(distinct 字段)
        // 分组获取第一条
        // 联表中GroupBy用法
        // ** 子查詢 **
        // ** 嵌套查詢 **
        // CRUD


        



    }

}
