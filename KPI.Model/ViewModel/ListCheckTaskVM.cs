using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class ListCheckTaskVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string KPILevelCode { get; set; }
        public string Area { get; set; }
        public int UserID { get; set; }
        public DateTime Deadline { get; set; }

        public int? Weekly { get; set; }

        public DateTime Monthly { get; set; }
        public DateTime Quarterly { get; set; }
        public DateTime Yearly { get; set; }

        public bool UpdateDataStatusW { get; set; }
        public bool UpdateDataStatusM { get; set; }
        public bool UpdateDataStatusQ { get; set; }
        public bool UpdateDataStatusY { get; set; }


        public bool WeeklyChecked { get; set; }
        public bool MonthlyChecked { get; set; }
        public bool QuarterlyChecked { get; set; }
        public bool YearlyChecked { get; set; }
    }
}
