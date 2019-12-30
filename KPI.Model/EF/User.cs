using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Code { get; set; }
        public string Username { get; set; }
        public string Skype { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string  Password { get; set; }
        public string FullName { get; set; }
        public bool State { get; set; }
        public int LevelID { get; set; }
        public int Role { get; set; }
        public int TeamID { get; set; }
        public bool IsActive { get; set; }
        public int Permission { get; set; }
        public int Owner { get; set; }
    }
}
