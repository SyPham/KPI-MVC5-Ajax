using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class ListActionPlanVM
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public int DataID { get; set; }
        public int CommentID { get; set; }

        public string Title { get; set; }
        public string KPILevelCodeAndPeriod { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public int TagID { get; set; }
        public int ApprovedBy { get; set; }

        public string Link { get; set; }
      
        public string Deadline { get; set; }
        public string SubmitDate { get; set; }

        public bool Status { get; set; }
        public bool ApprovedStatus { get; set; }
        public string Auditor { get; set; }
        public string UpdateSheduleDate { get; set; }
        public string ActualFinishDate { get; set; }
        public string Category { get; set; }
    }
}
