using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class UserSession
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool State { get; set; }
        public int LevelID { get; set; }
        public int Role { get; set; }
        public int TeamID { get; set; }
        public bool IsActive { get; set; }

    }
}
