using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class KPITreeViewModel
    {
        public KPITreeViewModel()
        {
            this.children = new List<KPITreeViewModel>();
        }

        public int key { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public int? levelnumber { get; set; }
        public int? parentid { get; set; }
        public bool state { get; set; }

        public bool HasChildren
        {
            get { return children.Any(); }
        }

        public List<KPITreeViewModel> children { get; set; }
    }
}
