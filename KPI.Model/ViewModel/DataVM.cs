using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI.Model.ViewModel
{
    public class DataVM
    {
        public int ID { get; set; }
        public string KPICode { get; set; }
        public int KPIKind { get; set; }
        public string Value { get; set; }
        public string Year { get; set; }
    }
}