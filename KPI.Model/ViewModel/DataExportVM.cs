using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class DataExportVM
    {
        public string KPILevelCode { get; set; }
        public string KPIName { get; set; }
        public string Value { get; set; }
        public int PeriodValueW { get; set; }
        public int PeriodValueM { get; set; }
        public int PeriodValueQ { get; set; }
        public int PeriodValueY { get; set; }
        public string Area { get; set; }

        public int? UploadTimeW { get; set; }
        public DateTime? UploadTimeM { get; set; }
        public DateTime? UploadTimeQ { get; set; }
        public DateTime? UploadTimeY { get; set; }

        public bool StateW { get; set; }
        public bool StateM { get; set; }
        public bool StateQ { get; set; }
        public bool StateY { get; set; }

        public string TargetValueW { get; set; }
        public string TargetValueM { get; set; }
        public string TargetValueQ { get; set; }
        public string TargetValueY { get; set; }

        public int Year { get; set; }
        public string Remark { get; set; }
    }
    public class DataExportVM2
    {
        public string KPILevelCode { get; set; }
        public string KPIName { get; set; }
        public int Value { get; set; }
        public int PeriodValueW { get; set; }
        public int PeriodValueM { get; set; }
        public int PeriodValueQ { get; set; }
        public int PeriodValueY { get; set; }
        public string Area { get; set; }

        public int UploadTimeW { get; set; }
        public DateTime? UploadTimeM { get; set; }
        public DateTime? UploadTimeQ { get; set; }
        public DateTime? UploadTimeY { get; set; }

        public bool StateW { get; set; }
        public bool StateM { get; set; }
        public bool StateQ { get; set; }
        public bool StateY { get; set; }

        public int Year { get; set; }
        public string Remark { get; set; }
    }
}
