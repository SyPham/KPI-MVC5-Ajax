using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class SubNotification
    {
        public int ID { get; set; }
        [DisplayName("OC")]
        public string Name { get; set; }
        public string KPIName { get; set; }

        public string URL { get; set; }
        public int UserID { get; set; }
        public int NotificationID { get; set; }
        public Notification Notification { get; set; }

    }
}
