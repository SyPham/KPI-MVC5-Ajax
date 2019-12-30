using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.Abstract
{
    public class Link : ILink
    {
       public int ID { get; set; }
       public string CodeTableA { get; set; }
       public string CodeTableB { get; set; }
    }
}
