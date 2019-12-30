using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class KPITreeViewModelAdminLevel
    {
        public KPITreeViewModelAdminLevel()
        {
            this.children = new List<KPITreeViewModelAdminLevel>();
        }

        public string key { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public int? levelnumber { get; set; }
        public string parentid { get; set; }
        public bool state { get; set; }

        public bool HasChildren
        {
            get { return children.Any(); }
        }

        public List<KPITreeViewModelAdminLevel> children { get; set; }
    }
}
