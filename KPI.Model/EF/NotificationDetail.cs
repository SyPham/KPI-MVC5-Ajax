using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class NotificationDetail
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int NotificationID { get; set; }
        public string Content { get; set; }
        public string URL { get; set; }
        public bool Seen { get; set; }
        public string Action { get; set; }
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
    }
}
