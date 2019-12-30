using KPI.Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class ActionPlanGettAllViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string Deadline { get; set; }
        public bool ApprovedStatus { get; set; }
        public bool Status { get; set; }
        public bool IsBoss { get; set; }
        public int CreatedBy { get; set; }
        public string UpdateSheduleDate { get; set; }
        public string ActualFinishDate { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public List<int> ListUserIDs { get; set; }
        public int Auditor { get; set; }

    }
}
