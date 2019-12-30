using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.ViewModel;
using MvcBreadCrumbs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class AdminLevelController : BaseController
    {
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("OC");
            var user = (UserProfileVM)Session["UserProfile"];
            if (user.User.Permission == 1)
            {
                return View();
            }
            else
            {
                return Redirect("~/Error/NotFound");
            }
        }

        public async Task<JsonResult> GetListTree()
        {
         
            return Json(await new LevelDAO().GetListTree(), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AddOrUpdate(Level entity)
        {
            return Json(await new LevelDAO().AddOrUpdate(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetByID(int id)
        {
            //string Code = JsonConvert.SerializeObject(code);
            return Json(await new LevelDAO().GetByID(id), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Rename(LevelActionVM level)
        {
            //string Code = JsonConvert.SerializeObject(code);
            return Json(await new LevelDAO().Rename(level), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Remove(int id)
        {
            //string Code = JsonConvert.SerializeObject(code);
            return Json(await new LevelDAO().Remove(id), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Add(Level level)
        {
            return Json(await new LevelDAO().Add(level), JsonRequestBehavior.AllowGet);
        }
    }
}