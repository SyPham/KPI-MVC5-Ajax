using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class CompareVM
    {
        public int? LevelNumber { get; set; }
        public string Area { get; set; }
        public bool Status { get; set; }
        public string KPILevelCode { get; set; }
        public bool StatusPublic { get; set; }

    }
}
