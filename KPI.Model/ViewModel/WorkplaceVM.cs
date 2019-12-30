using KPI.Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class WorkplaceVM
    {
        public int total { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public List<KPIUpLoadVM> KPIUpLoads { get; set; }
    }
    public class KPIUpLoadVM
    {
        public string KPIName { get; set; }
        public string Area { get; set; }
        public bool StateW { get; set; }
        public bool StateM { get; set; }
        public bool StateQ { get; set; }
        public bool StateY { get; set; }


        public bool StateDataW { get; set; }
        public bool StateDataM { get; set; }
        public bool StateDataQ { get; set; }
        public bool StateDataY { get; set; }
    }
}
