using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel.Menu
{
    public class MenuViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string FontAwesome { get; set; }
        public string BackgroudColor { get; set; }
        public int Permission { get; set; }
        public int ParentID { get; set; }
        public int Position { get; set; }
        public string LangID { get; set; }
        public string LangName { get; set; }
    }
}
