using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class NotificationViewModel
    {
        public int ID { get; set; }
        public int NotificationID { get; set; }
        public string Link { get; set; }
        public string Period { get; set; }
        public string KPIName { get; set; }
        public string Tag { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Seen { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DueDate { get; set; }
        public DateTime Deadline { get; set; }
        public int PIC { get; set; }
        public int Owner { get; set; }
        public int Manager { get; set; }
        public int Sponsor { get; set; }
        public int[] TagComment { get; set; }
        public int[] TagActionPlan { get; set; }
        public int[] Tags { get; set; }
        public string Action { get; set; }

        public string Recipient { get; set; }
        public string Sender { get; set; }
        public int RecipientID { get; set; }
        public int SenderID { get; set; }
        public string TaskName { get; set; }

        public string URL { get; set; }
        public string ContentDetail { get; set; }
    }
}
