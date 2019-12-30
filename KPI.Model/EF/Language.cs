using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class Language
    {
        [Key]
        [StringLength(2)]
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
