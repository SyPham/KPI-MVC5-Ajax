using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class Level
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentID { get; set; }
        public string ParentCode { get; set; }
        public int? LevelNumber { get; set; }
        public bool State { get; set; }
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
