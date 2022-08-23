using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Entity
{
    internal class ScoreData
    {
        public string? CourseName { get; set; }
        public decimal? MaxScore { get; set; }
        public decimal? MinScore { get; set; }
        public decimal? AvgScore { get; set; }
    }
}
