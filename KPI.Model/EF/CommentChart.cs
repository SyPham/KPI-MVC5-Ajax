using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class CommentChart
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public int Creator { get; set; }
        public DateTime Modified { get; set; }
        public int Item { get; set; }
        public string OriginalItemHistoryID { get; set; }
        public int Parent { get; set; }
        public string Content { get; set; }

        public string Pings { get; set; }
        public string File { get; set; }
        public string FileURL { get; set; }
        public string FileMimeType { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool CreateByStaff { get; set; }
        public bool CreatedByCurrentUser { get; set; }
       
        public int UpvoteCount { get; set; }
        public bool UserHasUpvoted { get; set; }
       
        public int DataID { get; set; }
        public int UserID { get; set; }
    }
}
      