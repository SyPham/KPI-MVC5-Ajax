using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
  public class AddCommentVM
    {
        public bool Status { get; set; }
        public List<string[]> ListEmails { get; set; }
        public string Message { get; set; }
    }
    
}
