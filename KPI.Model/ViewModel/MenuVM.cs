using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class MenuVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string FontAwesome { get; set; }
        public string BackgroudColor { get; set; }
        public int ParentID { get; set; }
        public int Position { get; set; }

        public string PermissionName { get; set; }
        public List<MenuVM> children { get; set; }
    }
}
