using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KPI.Web.Controllers
{
    public class BaseController : Controller
    {
      
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var returnUrl = filterContext.HttpContext.Request.UrlReferrer;
            if (returnUrl == null)
                returnUrl = filterContext.HttpContext.Request.Url;
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index" , returnUrl }));
            }
            base.OnActionExecuting(filterContext);
        }

    }
} 