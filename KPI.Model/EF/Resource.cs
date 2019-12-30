using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class Resource
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public string  Name { get; set; }
        public string FontAwesome { get; set; }
        public string BackgroudColor { get; set; }
        public bool CheckRole { get; set; }
        public bool State { get; set; }
        public int Permission { get; set; }
        public int Menu { get; set; }
        public int UserID { get; set; }
    }
}
