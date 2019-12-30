using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI.Web.Models
{
    public class DataExport
    {
        
        public string KPICode  { get; set; }
        public string Value{ get; set; }
        public string DateUpload { get; set; }
        public string Year { get; set; }
    }
}