using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KPI.Web.Models.MenuViewModel
{
    public class MenuViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string FontAwesome { get; set; }
        public string BackgroudColor { get; set; }
        public int Permission { get; set; }
        public int ParentID { get; set; }
        public int Position { get; set; }

        public string LangID { get; set; }
        [DisplayName("English Name")]
        public string LangNameEn { get; set; }
        [DisplayName("Chinese Name")]
        public string LangNameTw { get; set; }
        [DisplayName("Vietnamese Name")]
        public string LangNameVi { get; set; }

    }
}