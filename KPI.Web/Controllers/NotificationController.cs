using KPI.Model.DAO;
using KPI.Model.ViewModel;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    public class NotificationController : BaseController
    {
        // GET: AddUserToLevel
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> UpdateRange(string listID)
        {
            return Json(await new NotificationDAO().UpdateRange(listID), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Update(int ID)
        {
            var obj =await new NotificationDAO().Update(ID);
            NotificationHub.SendNotifications();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}