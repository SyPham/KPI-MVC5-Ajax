using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class ActionPlanVM
    {
        public int ActionplanID { get; set; }
        public DateTime Deadline { get; set; }
        public string Email { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public int KPILevelID { get; set; }
        public bool Sent { get; set; }
    }
}
