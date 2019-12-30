using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class Setting
    {
        public Setting()
        {
            CreatedTime = DateTime.Today;
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
