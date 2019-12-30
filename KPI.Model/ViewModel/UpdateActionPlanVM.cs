using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class UpdateActionPlanVM
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public string DeadLine { get; set; }

        public string Description { get; set; }

        public string Tag { get; set; }

    }
}
