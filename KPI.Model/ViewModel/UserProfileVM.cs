using KPI.Model.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KPI.Model.ViewModel
{
    [Serializable()]
    public  class UserProfileVM 
    {
        public User User { get; set; }
        public List<ViewModel.Menu.MenuViewModel> Menus { get; set; }
    }
}
