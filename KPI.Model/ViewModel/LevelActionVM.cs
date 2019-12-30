using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class LevelActionVM
    {
        public int key { get; set; }
        public string code { get; set; }
        public string title { get; set; }

        public int levelnumber { get; set; }
        public int parentid { get; set; }
    }
}
