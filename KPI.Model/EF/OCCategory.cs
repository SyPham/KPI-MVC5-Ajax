using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class OCCategory
    {
        public int ID { get; set; }
        public int OCID { get; set; }
        public int CategoryID { get; set; }
        public bool Status { get; set; }
    }
}
