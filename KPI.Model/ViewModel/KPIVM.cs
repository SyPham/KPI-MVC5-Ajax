using KPI.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class KPIVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int LevelID { get; set; }
        public string CategoryName { get; set; }
        public string Unit { get; set; }
        public int CategoryID { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
