using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class UploadKPIVM
    {
        public string KPILevelCode { get; set; }
        public string Area { get; set; }
        public string KPIName { get; set; }
        public string Email { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public bool Status { get; set; }

    }
    public class ImportDataVM
    {
        public List<UploadKPIVM> ListUploadKPIVMs { get; set; }
        public List<UploadKPIVM> ListDataSuccess { get; set; }
        public bool Status { get; set; }
        public List<string> ListSendMail { get; set; }
    }
}