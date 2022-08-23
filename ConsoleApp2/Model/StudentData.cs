using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Model
{
    internal class StudentData
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity = true)] //设置主键
        public int? ID { get; set; }

        public string? Name { get; set; }

        public string? City { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
