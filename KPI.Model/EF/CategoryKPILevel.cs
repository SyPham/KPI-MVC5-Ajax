using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class CategoryKPILevel
    {
        public int ID { get; set; }
        public int KPILevelID { get; set; }
        public int CategoryID { get; set; }
        public bool Status { get; set; }
    }
}
