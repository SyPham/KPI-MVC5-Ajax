using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class ListDataUploadVM
    {
        public int KPILevelID { get; set; }
        public string KPILevelCode { get; set; }
        public string KPIName { get; set; }
        public bool? WeeklyChecked { get; set; }
        public bool? MonthlyChecked { get; set; }
        public bool? QuarterlyChecked { get; set; }
        public bool? YearlyChecked { get; set; }
        public int? StateW { get; set; }
        public int? StateM { get; set; }
        public int? StateQ { get; set; }
        public int? StateY { get; set; }
    }
}
