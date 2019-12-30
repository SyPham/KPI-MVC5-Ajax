using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class ManagerOwnerUpdaterSponsorParticipantVM
    {
        public int KPILevelID{ get; set; }
        public string KPILevelCode { get; set; }
        public int CategoryID { get; set; }
        public string KPIName { get; set; }
        public string Owner { get; set; }
        public string Manager { get; set; }
        public string Updater { get; set; }
        public string Sponsor { get; set; }
        public string Participant { get; set; }
    }
}
