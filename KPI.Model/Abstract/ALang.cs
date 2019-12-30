using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.Abstract
{
   public abstract class ALang
    {
        public ALang()
        {
            CreatedTime = DateTime.Now;
        }

        public int ID { get; set; }
        public string LangID { get; set; }
        public string Name { get; set; }
        public int MenuID { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
