using KPI.Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace KPI.Model.helpers
{
    public static class CookieUtility
    {
        public static User UserCookie
        {
            get
            {
                var itemCookie = HttpContext.Current.Request.Cookies["UserCookie"];
                if (itemCookie != null)
                {
                    User userCookie = new JavaScriptSerializer().Deserialize<User>(itemCookie.Value);
                    return userCookie;
                }
                else
                    return null;
            }
            set
            {
                string userJson = new JavaScriptSerializer().Serialize(value);
                HttpCookie userCookie = new HttpCookie("UserCookie", userJson)
                {
                    Expires = DateTime.Now.AddDays(30)
                };
                HttpContext.Current.Response.Cookies.Add(userCookie);
            }
        }

        public static void ExpireAllCookies()
        {
            if (HttpContext.Current != null)
            {
                int cookieCount = HttpContext.Current.Request.Cookies.Count;
                for (var i = 0; i < cookieCount; i++)
                {
                    var cookie = HttpContext.Current.Request.Cookies[i];
                    if (cookie != null)
                    {
                        var expiredCookie = new HttpCookie(cookie.Name)
                        {
                            Expires = DateTime.Now.AddDays(-1),
                            Domain = cookie.Domain
                        };
                        HttpContext.Current.Response.Cookies.Add(expiredCookie);
                    }
                }

                // clear cookies server side
                HttpContext.Current.Request.Cookies.Clear();
            }
        }
    }
}
