using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class StateSendMail
    {
        public StateSendMail()
        {
            ToDay = DateTime.Today;
            Status = true;
            Description = "IsSended";
        }

        public int ID { get; set; }
        public DateTime ToDay { get; 
            set; }
        public bool Status { get; set; }
        public string Description { get; set; }
    }
}
