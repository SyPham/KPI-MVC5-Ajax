using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI.Web.helpers
{
    public static class SessionUtility
    {
        public static User User
        {
            get
            {
                return HttpContext.Current.Session["User"] as User;
            }
            set
            {
                HttpContext.Current.Session["User"] = value;
            }
        }

       
        //public static string CurrentCulture { get; set; }

        public static string CurrentCulture
        {
            get
            {
                if (HttpContext.Current.Session["CurrentCulture"] == null)
                {
                    HttpContext.Current.Session["CurrentCulture"] = "vi";
                }

                return HttpContext.Current.Session["CurrentCulture"].ToSafetyString();
            }
            set
            {
                HttpContext.Current.Session["CurrentCulture"] = value;
            }
        }
    }
}