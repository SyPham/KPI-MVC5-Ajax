using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI.Web.Models
{
    public class AppUserViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool State { get; set; }
        public int LevelID { get; set; }
        public int Role { get; set; }
        public int TeamID { get; set; }
        public bool IsActive { get; set; }
        public int Permission { get; set; }
    }
}