using KPI.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class KPI : Inheritance
    {
        public string CategoryCode { get; set; }
        public int CategoryID { get; set; }
        public int Unit { get; set; }
    }
}
