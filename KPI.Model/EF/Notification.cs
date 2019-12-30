using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    [Serializable]
    public class Notification 
    {
        public Notification()
        {
            this.CreateTime = DateTime.Now;
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public string KPIName { get; set; }
        public string Period { get; set; }
        [Column("Action")]
        public string Action { get; set; }
        public string Link { get; set; }
        public DateTime CreateTime { get; set; }
        public string Tag { get; set; }
        public string KPILevelCode { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CommentID { get; set; }
        public int ActionplanID { get; set; }
        public string TaskName { get; set; }
        public virtual ICollection<SubNotification> SubNotifications { get; set; }
    }
}
