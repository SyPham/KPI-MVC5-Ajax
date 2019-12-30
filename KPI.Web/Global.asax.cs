using KPI.Model;
using KPI.Model.DAO;
using KPI.Model.ViewModel;
using KPI.Web.WindowService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KPI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected  void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Application["user_online"] = 0;
            //await JobScheduler.StartAsync();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Lang"];
            if (cookie != null && cookie.Value != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
            }
            else
            {
                HttpCookie cooki = new HttpCookie("Lang");
                cooki.Value = "en";
                Response.Cookies.Add(cooki);

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
           
            Application.Lock();
            Application["user_online"] = (int)Application["user_online"] + 1;
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["user_online"] = (int)Application["user_online"] - 1;
            Application.UnLock();
        }
    }

}

