using KPI.Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class ChartVM
    {
        public string[] datasets { get; set; }
        public string[] labels { get; set; }
        public string[] targets { get; set; }
        public int[] standards { get; set; }

        public string label { get; set; }
        public string kpiname { get; set; }
        public string period { get; set; }
        public string kpilevelcode { get; set; }
        public bool statusfavorite { get; set; }
        public int Standard { get; set; }
        public string Unit { get; set; }
        public int[] dataids { get; set; }

        public List<int> datasets1 { get; set; }
        public List<string> labels1 { get; set; }

        public List<Dataremark> Dataremarks { get; set; }

        public object PIC { get; set; }
        public object Owner { get; set; }
        public object OwnerManagerment { get; set; }
        public object Sponsor { get; set; }
        public object Participant { get; set; }
    }
    public class ChartVM2
    {
        public string[] datasets { get; set; }
        public string[] labels { get; set; }
        public string[] targets { get; set; }
        public int[] standards { get; set; }
        public int Standard { get; set; }
        public string label { get; set; }
        public string kpiname { get; set; }
        public string period { get; set; }
        public string kpilevelcode { get; set; }
        public string Unit { get; set; }
    }
    public class Dataremark
    {
        public int ID { get; set; }
        public string KPILevelCode { get; set; }
        public string KPIKind { get; set; }
        public string Value { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Quater { get; set; }
        public int Year { get; set; }
        public string DateUpload { get; set; }
        public string Remark { get; set; }
        public string Target { get; set; }
        public string [] ValueArray { get; set; }
        
    }
   
}
