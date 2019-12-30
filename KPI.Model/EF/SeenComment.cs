using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class SeenComment
    {
        public int ID { get; set; }
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public bool Status { get; set; }
    }
}
