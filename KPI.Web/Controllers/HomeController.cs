using KPI.Model.DAO;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using KPI.Model.EF;
using KPI.Web.helpers;
using KPI.Model;
using System.Globalization;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class HomeController : BaseController
    {
        [BreadCrumb(Clear = true)]
        public async Task<ActionResult> Index()
        {
            BreadCrumb.Add("/", "Home");
            BreadCrumb.SetLabel("Dashboard");
            //if (!await new NotificationDAO().IsSended())
            //{
            //    //var model2 = new ActionPlanDAO().CheckLateOnUpdateData();
            //var model = new ActionPlanDAO().CheckDeadline();
            //string from = ConfigurationManager.AppSettings["FromEmailAddress"].ToSafetyString();
            ////Late on upload data
            //string content2 = System.IO.File.ReadAllText(Server.MapPath("~/Templates/LateOnUpDateData.html"));
            //content2.Replace("{{{content}}}", "Your below KPIs have expired: ");

            ////Late on task
            //string content = System.IO.File.ReadAllText(Server.MapPath("~/Templates/LateOnUpDateData.html"));
            //content.Replace("{{{content}}}", "Your below KPIs have expired: ");
            //var html = string.Empty;
            //var count = 0;
            //foreach (var item2 in model2)
            //{
            //    count++;
            //    html += @"<tr>
            //            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{no}}</td>
            //            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{kpiname}}</td>
            //            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{year}}</td>
            //             </tr>"
            //            .Replace("{{no}}", count.ToString())
            //            .Replace("{{kpiname}}", item2[1])
            //            .Replace("{{year}}", item2[3]);
            //    content2 = content2.Replace("{{{html-template}}}", html);
            //    string to = item2[0].ToSafetyString();
            //    string subject = "Late on upload data";
            //    Commons.SendMail(from, to, subject, content2, "Late on upload data");
            //}

            //foreach (var item in model)
            //{
            //    //string content = "Please note that the action plan we are overdue on " + item.Deadline;
            //    count++;
            //    html += @"<tr>
            //            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{no}}</td>
            //            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{kpiname}}</td>
            //            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{year}}</td>
            //             </tr>"
            //            .Replace("{{no}}", count.ToString())
            //            .Replace("{{kpiname}}", item.Title);

            //    content = content.Replace("{{{html-template}}}", html);
            //    string to = item.Email.ToSafetyString();
            //    string subject = "Late on task";
            //    Commons.SendMail(from, to, subject, content, "Late on task ");
            //}
            //var itemSendMail = new StateSendMail();
            //await new NotificationDAO().AddSendMail(itemSendMail);
            //}
            await SendMail();
            return View();
        }
        public ActionResult ChangeLanguage(String Langby)
        {
            if (Langby != null)
            {
                var userProfile = Session["UserProfile"] as UserProfileVM;

                var sibars = new UserAdminDAO().Sidebars(userProfile.User.Permission, Langby);
                var userVM = new UserProfileVM();
                userVM.User = userProfile.User;
                userVM.Menus = sibars;

                Session["UserProfile"] = userVM;
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Langby);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(Langby);
            }

            HttpCookie cookie = new HttpCookie("Lang");
            cookie.Value = Langby;
            Response.Cookies.Add(cookie);
            string urlReferrer = Request.UrlReferrer.ToString();

            return Redirect(Request.UrlReferrer.ToString());
        }
        public async Task SendMail()
        {
            if (!await new NotificationDAO().IsSend())
            {
                #region *) Field

                var userprofile = Session["UserProfile"] as UserProfileVM;
                string host = ConfigurationManager.AppSettings["Http"].ToSafetyString();
                string contentForAuditUpload = System.IO.File.ReadAllText(Server.MapPath("~/Templates/LateOnUpDateData.html"));
                contentForAuditUpload = contentForAuditUpload.Replace("{{{content}}}", @"<b style='color:red'>Late On Update Data</b><br/>Your KPIs have expired as below list: ");

                string contentForDeadline = System.IO.File.ReadAllText(Server.MapPath("~/Templates/LateOnTask.html"));
                contentForDeadline = contentForDeadline.Replace("{{{content}}}", @"<b style='color:red'>Late On Task</b><br/>Your task have expired as below list: ");
                var htmlForUpload = string.Empty;
                var htmlForDeadLine = string.Empty;

                var count = 0;
                var auditUploadModel = new ActionPlanDAO().CheckLateOnUpdateData(userprofile.User.ID);
                var model = new ActionPlanDAO().CheckDeadline();

                #endregion

                #region 1) Late On Update
                if (await new SettingDAO().IsSendMail("CHECKLATEONUPDATEDATA"))
                {


                    foreach (var item2 in auditUploadModel.Item1)
                    {
                        count++;
                        htmlForUpload += @"<tr>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{no}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{area}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{ockpicode}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{kpiname}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{year}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{deadline}}</td>
                             </tr>"
                                .Replace("{{no}}", count.ToSafetyString())
                                .Replace("{{area}}", item2[3].ToSafetyString())
                                .Replace("{{kpiname}}", item2[0].ToSafetyString())
                                .Replace("{{ockpicode}}", item2[2].ToSafetyString())
                                .Replace("{{year}}", item2[4].ToSafetyString())
                                .Replace("{{deadline}}", item2[1].ToSafetyString());
                    }
                    contentForAuditUpload = contentForAuditUpload.Replace("{{{html-template}}}", htmlForUpload);
                    if (auditUploadModel.Item1.Count > 0)
                        Commons.SendMail(auditUploadModel.Item2.Select(x => x.Email).ToList(), "[KPI System] Late on upload data", contentForAuditUpload, "Late on upload data");

                }
                #endregion

                #region 2) Check Dead Line
                if (await new SettingDAO().IsSendMail("CHECKDEADLINE"))
                {
                    foreach (var item in model.Item1)
                    {
                        //string content = "Please note that the action plan we are overdue on " + item.Deadline;
                        count++;
                        htmlForDeadLine += @"<tr>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{no}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{kpiname}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{deadline}}</td>
                             </tr>"
                                .Replace("{{no}}", count.ToString())
                                .Replace("{{kpiname}}", item[0].ToSafetyString())
                                .Replace("{{deadline}}", item[1].ToSafetyString("MM/dd/yyyy"));
                    }
                    contentForDeadline = contentForDeadline.Replace("{{{html-template}}}", htmlForDeadLine);
                    if (model.Item1.Count > 0)
                        Commons.SendMail(model.Item2.Select(x => x.Email).ToList(), "[KPI System] Late on task", contentForDeadline, "Late on task ");

                }
                #endregion

                #region *) Thông báo để biết gửi mail hay chưa

                var itemSendMail = new StateSendMail();
                await new NotificationDAO().AddSendMail(itemSendMail);
                new ErrorMessageDAO().Add(new ErrorMessage
                {
                    Function = "Send Mail",
                    Name = "[KPI System] Late on task, [KPI System] Late on upload data"
                });
                #endregion
            }
        }
        public ActionResult UserDashBoard()
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;

            if (userprofile != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult LateOnUpload(int notificationId)
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            return View(new DataChartDAO().LateOnUpLoads(userprofile.User.ID, notificationId));

        }

        public async Task<ActionResult> header()
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile != null)
            {
                var collection = await new NotificationDAO().NotifyCollection();
                return PartialView(collection);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public async Task<JsonResult> GetNotifications()
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile.User.ID == 1)
            {
                return Json(new { arrayID = new List<int>(), total = 0, data = new List<NotificationViewModel>() }, JsonRequestBehavior.AllowGet);

            }
            var listNotifications = await new NotificationDAO().ListNotifications(userprofile.User.ID);
            var total = 0;
            var listID = new List<int>();
            foreach (var item in listNotifications)
            {
                if (item.Seen == false)
                {
                    total++;
                    listID.Add(item.ID);
                }

            }
            return Json(new { arrayID = listID.ToArray(), total = total, data = listNotifications }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListHistoryNotification()
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile == null)
                return Json("", JsonRequestBehavior.AllowGet);
            IEnumerable<NotificationViewModel> model = new NotificationDAO().GetHistoryNotification(userprofile.User.ID);
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> ListSubNotificationDetail(int notificationId)
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            return View(await new UploadDAO().GetAllSubNotificationsByIdAsync(notificationId, userprofile.User.ID));
        }

    }
}

