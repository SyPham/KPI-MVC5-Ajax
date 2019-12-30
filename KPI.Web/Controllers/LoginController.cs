using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Web.helpers;
using KPI.Model.ViewModel;
using KPI.Web.Common;
using KPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using KPI.Web.Extension;

namespace KPI.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        //public ActionResult Index()
        //{
        //   //var model= new LevelDAO().GetNode(97);
        //    return View();
        //}
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            returnUrl = Request.UrlReferrer.ToSafetyString();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            
            string[] routers = new[] { "Workplace", "CategoryKPILevel", "Favourite","ChartPeriod" };
            var userprofile = Session["UserProfile"] as UserProfileVM;
            //kiem tra neu la admin thi chuyen den router khac
           

            if (!returnUrl.IsNullOrEmpty())
            {
                bool flag = false;

                foreach (var item in routers)
                {
                    //Khong nam trong router cua user flag = true
                    var check = returnUrl.ToLower().IndexOf(item.ToLower());
                    if (check != -1)
                        flag = true;
                }
                //Neu la admin va k router khong nam trong router cua user, Redirect home/index
                //Neu KHONG la admin ,Redirect home/index

                if (userprofile?.User.Permission == 1 && flag)
                    return RedirectToAction("Index", "Home");
                else if (userprofile?.User.Permission != 1)
                {
                    
                    if (flag)
                    {
                        Uri myUri = new Uri(Request.UrlReferrer.ToSafetyString());
                        string returnUrl2 = HttpUtility.ParseQueryString(myUri.Query).Get("returnUrl");
                        return Redirect(returnUrl2);
                    }
                    else
                    {
                       
                        return RedirectToAction("Index", "Home");

                    }
                }
                else
                {
                    Uri myUri = new Uri(Request.UrlReferrer.ToSafetyString());
                    string returnUrl2 = HttpUtility.ParseQueryString(myUri.Query).Get("returnUrl");
                    return Redirect(returnUrl2);
                }
               
            }
            else
            {
                if(Request.UrlReferrer != null)
                {
                    Uri myUri = new Uri(Request.UrlReferrer.ToSafetyString());
                    string returnUrl2 = HttpUtility.ParseQueryString(myUri.Query).Get("returnUrl");
                    return Redirect(returnUrl2);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
        }
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(UserProfile objUser, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if(await new UserLoginDAO().CheckExistsUser(objUser.Username, objUser.Password))
                {
                    var obj = await new UserLoginDAO().GetUserProfile(objUser.Username, objUser.Password);
                    if (obj != null)
                    {
                        Session["UserProfile"] = obj as UserProfileVM;
                        Session.Timeout = 525600;
                            return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }

                }
                ViewBag.Error = "Username or Pasword is wrong!";
                ViewBag.Status = false;
               // this.Response.Redirect(Server.UrlEncode(Request.QueryString["returnUrl"].ToString()));

                return View(objUser);
            }
            ViewBag.ReturnUrl = returnUrl;

            return View(objUser);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("/");
        }
        
    }
}