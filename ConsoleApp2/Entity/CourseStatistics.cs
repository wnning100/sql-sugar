using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Entity
{
    internal class CourseStatistics
    {
        public string? CourseID { get; set; }
        public string? CourseName { get; set; }

        public decimal Case100To85 { get; set; }

        public decimal Case85To70 { get; set; }
        public decimal Case70To60 { get; set; }
        public decimal CaseFail { get; set; }
    }
}
