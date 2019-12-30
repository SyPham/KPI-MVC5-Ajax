using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class AddCommentViewModel
    {
        public string CommentMsg { get; set; }
        public int UserID { get; set; }
        public int DataID { get; set; }
        public string Tag { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string KPILevelCode { get; set; }
        public int CategoryID { get; set; }
    }

}
