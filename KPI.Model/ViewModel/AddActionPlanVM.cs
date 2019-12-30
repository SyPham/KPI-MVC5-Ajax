using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class AddActionPlanVM
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int DataID { get; set; }
        public int CommentID { get; set; }

        public string Title { get; set; }
        public string KPILevelCodeAndPeriod { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public int TagID { get; set; }
        public int ApprovedBy { get; set; }

        public string Link { get; set; }
        private DateTime? createTime = null;
        public DateTime CreateTime
        {
            get
            {
                return this.createTime.HasValue
                   ? this.createTime.Value
                   : DateTime.Now;
            }

            set { this.createTime = value; }
        }
        public DateTime Deadline { get; set; }
        public DateTime SubmitDate { get; set; }

        public bool Status { get; set; }
        public bool ApprovedStatus { get; set; }
        public string Auditor { get; set; }
        public DateTime? UpdateSheduleDate { get; set; }
        public DateTime? ActuralFinishDate { get; set; }
    }
}
