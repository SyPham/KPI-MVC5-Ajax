using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using KPI.Web.helpers;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class ChartPeriodController : BaseController
    {
       
        // GET: Month
        [BreadCrumb(Clear = true)]
        public async Task<ActionResult> Index(string kpilevelcode, int? catid, string period, int? year, int? start, int? end)
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.Add("/KPI/Index", "KPI");
            if (period == "W")
            {
                BreadCrumb.SetLabel("Chart / Weekly");
            }
            else if (period == "M")
            {
                BreadCrumb.SetLabel("Chart / Monthly");
            }
            else if (period == "Q")
            {
                BreadCrumb.SetLabel("Chart / Quarterly");
            }
            else if (period == "Y")
            {
                BreadCrumb.SetLabel("Chart / Yearly");
            }

            var model = await new DataChartDAO().ListDatas(kpilevelcode, catid, period, year, start, end);
            ViewBag.Model = model;
            return View();
        }

        public async Task<JsonResult> AddComment(AddCommentViewModel entity)
        {
            var sessionUser = Session["UserProfile"] as UserProfileVM;
            int levelNumberOfUserComment = sessionUser.User.LevelID;
            var data = await new KPILevelDAO().AddComment(entity, levelNumberOfUserComment);
            var tos = new List<string>();
            NotificationHub.SendNotifications();
            if (data.ListEmails.Count > 0 && await new SettingDAO().IsSendMail("ADDCOMMENT"))
            {
                var model = data.ListEmails.DistinctBy(x => x);
                string content = "The account " + model.First()[0] + " mentioned you in KPI System Apps. Content: " + model.First()[4] + ". " + model.First()[3] + " Link: " + entity.Link;
                Commons.SendMail(model.Select(x=> x[1]).ToList(), "[KPI System] Comment", content, "Comment");
            }
            return Json(new { status = data.Status, isSendmail = true }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> LoadDataComment(int dataid, int userid)
        {
            return Json(await new KPILevelDAO().ListComments(dataid, userid), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AddCommentHistory(int userid, int dataid)
        {
            return Json(await new KPILevelDAO().AddCommentHistory(userid, dataid), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Remark(int dataid)
        {
            return Json(await new DataChartDAO().Remark(dataid), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AddFavourite(Model.EF.Favourite entity)
        {
            return Json(await new FavouriteDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> LoadDataProvide(string obj, int page, int pageSize)
        {
            return Json(await new KPILevelDAO().LoadDataProvide(obj, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UpdateRemark(int dataid, string remark)
        {
            return Json(await new DataChartDAO().UpdateRemark(dataid, remark), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Update(ActionPlan item)
        {
            return Json(await new ActionPlanDAO().Update(item), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Add(AddActionPlanViewModel obj)
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            obj.OwnerID = userprofile.User.ID;
            var data = await new ActionPlanDAO().Add(obj);//(item, obj.Subject, obj.Auditor, obj.CategoryID);
            NotificationHub.SendNotifications();
            if (data.ListEmails.Count > 0 && await new SettingDAO().IsSendMail("ADDTASK"))
            {
                string content = "The account " + data.ListEmails.First()[0] + " mentioned you in KPI System Apps. Content: " + data.ListEmails.First()[4] + ". " + data.ListEmails.First()[3] + " Link: " + obj.Link;
                Commons.SendMail(data.ListEmails.Select(x => x[1]).ToList(), "[KPI System] Add task", content, "Action Plan (Add Task)");
            }
            return Json(new { status = data.Status, isSendmail = true, message = data.Message }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Delete(int id)
        {
            return Json(await new ActionPlanDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetAll(int DataID, int CommentID, int UserID)
        {
            //var userprofile = Session["UserProfile"] as UserProfileVM;
            return Json(await new ActionPlanDAO().GetAll(DataID, CommentID, UserID), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetByID(int id)
        {
            return Json(await new ActionPlanDAO().GetByID(id), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Approval(int id, int approveby, string KPILevelCode, int CategoryID, string url)
        {
            var model = await new ActionPlanDAO().Approval(id, approveby, KPILevelCode, CategoryID);
            NotificationHub.SendNotifications();
            if (model.Item1.Count > 0 && await new SettingDAO().IsSendMail("APPROVAL"))
            {
                var data = model.Item1.DistinctBy(x => x);
                string content = "The account " + data.First()[0] + " was approved the task " + data.First()[3] + " Link: " + url;
               Commons.SendMail(data.Select(x => x[1]).ToList(), "[KPI System] Approved", content, "Action Plan (Approved)");
            }
            return Json(new { status = model.Item2, isSendmail = true }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Done(int id, string KPILevelCode, int CategoryID, string url)
        {
            
            var userprofile = Session["UserProfile"] as UserProfileVM;
            var model = await new ActionPlanDAO().Done(id, userprofile.User.ID, KPILevelCode, CategoryID);
            NotificationHub.SendNotifications();
            if (model.Item1.Count > 0 && await new SettingDAO().IsSendMail("DONE"))
            {
                var data = model.Item1.DistinctBy(x => x);
                string content = "The account " + data.First()[0] + " has finished the task" + data.First()[3] + " Link: " + url;
               Commons.SendMail(data.Select(x => x[1]).ToList(), "[KPI System] Action Plan", content, "Action Plan (Finished Task)");
            }
            return Json(new { status = model.Item2, isSendmail = true }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AddNotification(Notification notification)
        {
            var status = await new NotificationDAO().Add(notification);
            NotificationHub.SendNotifications();
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UpdateActionPlan(UpdateActionPlanVM actionPlan)
        {
            return Json(await new ActionPlanDAO().UpdateActionPlan(actionPlan), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UpdateSheduleDate(string name, string value, string pk)
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            return Json(await new ActionPlanDAO().UpdateSheduleDate(name, value, pk, userprofile.User.ID), JsonRequestBehavior.AllowGet);
        }

        //public async Task<JsonResult> GetAllDataByCategory(int catid, string period, int? year)
        //{
        //    var currenYear = year ?? DateTime.Now.Year;
        //    return Json(new DataChartDAO().GetAllDataByCategory(catid, period, currenYear), JsonRequestBehavior.AllowGet);

        //}
    }
}