using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class Comment
    {
        public int ID { get; set; }
        public string CommentMsg { get; set; }
        public int UserID { get; set; }
        
        public int DataID { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        private DateTime? commentedDate = null;
        public DateTime CommentedDate
        {
            get
            {
                return this.commentedDate.HasValue
                   ? this.commentedDate.Value
                   : DateTime.Now;
            }

            set { this.commentedDate = value; }
        }
      
    }
}
