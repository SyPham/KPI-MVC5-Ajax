using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class DataCompareVM
    {
        public ChartVM list1 { get; set; }
        public ChartVM list2 { get; set; }
        public ChartVM list3 { get; set; }
        public ChartVM list4 { get; set; }
        public int Standard { get; set; }
        public string Unit { get; set; }
        public string Period { get; set; }
    }

}
