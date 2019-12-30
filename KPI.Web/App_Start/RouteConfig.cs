using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KPI.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                 name: "LateOnUpload",
                 url: "Home/LateOnUpload/{notificationId}",
                 defaults: new { controller = "Home", action = "LateOnUpload", notificationId = UrlParameter.Optional }
             );
            routes.MapRoute(
               name: "404-NotFound",
               url: "NotFound",
               defaults: new { controller = "Error", action = "NotFound" }
           );

            routes.MapRoute(
                name: "500-Error",
                url: "Error",
                defaults: new { controller = "Error", action = "Error" }
            );

            routes.MapRoute(
                name: "dataset",
                url: "dataset/{catid}/{period}/{start}/{end}/{year}",
                defaults: new { 
                    controller = "Dataset", 
                    action = "Index", 
                    catid = UrlParameter.Optional, 
                    period = UrlParameter.Optional,
                    start = UrlParameter.Optional,
                    end  = UrlParameter.Optional,
                    year = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
           
        }
    }
}
