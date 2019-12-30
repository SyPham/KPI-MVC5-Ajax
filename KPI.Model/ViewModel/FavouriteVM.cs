using KPI.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class FavouriteVM 
    {
        public int ID { get; set; }
        public string KPIName { get; set; }
        public string Username { get; set; }
        public string TeamName { get; set; }
        public string KPILevelCode { get; set; }
        public string Period { get; set; }
        public int Level { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
