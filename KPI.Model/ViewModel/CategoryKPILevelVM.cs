using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class CategoryKPILevelVM
    {
        public int KPIID { get; set; }
        public int CategoryID { get; set; }
        public int KPILevelID { get; set; }
        public string KPILevelCode { get; set; }
    }
}
