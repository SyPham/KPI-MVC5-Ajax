using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class DatasetVM
    {
        public string CategoryCode { get; set; }

        public string KPIName { get; set; }
        public string KPILevelCode { get; set; }
        public int Target { get; set; }
        public string Period { get; set; }
        public string CategoryName { get; set; }
        public object Datasets { get; set; }
        public string Owner { get; set; }
        public string Manager { get; set; }
        public string Updater { get; set; }
        public string Sponsor { get; set; }
        public string Participant { get; set; }
        public object KPIObj { get; set; }
    }
    public class Genaral {
        public string Owner { get; set; }
        public string Manager { get; set; }
        public string Updater { get; set; }
        public string Sponsor { get; set; }
        public string Participant { get; set; }
    }
}
