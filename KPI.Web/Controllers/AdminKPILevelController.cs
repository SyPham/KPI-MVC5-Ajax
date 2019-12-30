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
    [BreadCrumb(Clear = true)]
    public class AdminKPILevelController : BaseController
    {
        // GET: AdminKPILevel
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("KPI OC");
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
        public async Task<JsonResult> LoadDataKPILevel(int level, int category, int page, int pageSize)
        {
            return Json(await new KPILevelDAO().LoadData(level, category, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetCategoryCode(Model.EF.Category entity)
        {
            return Json(await new KPILevelDAO().GetAllCategory(), JsonRequestBehavior.AllowGet);
        }
        //update kpiLevel
        public async Task<JsonResult> UpdateKPILevel(UpdateKPILevelVM entity)
        {
            //var user = (UserProfileVM)Session["UserProfile"];
            //entity.UserCheck = user.User.ID.ToString();
            return Json(await new KPILevelDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetbyID(int ID)
        {
            return Json(await new KPILevelDAO().GetDetail(ID), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetListAllUser()
        {
            return Json(await new UserAdminDAO().GetListAllUser(), JsonRequestBehavior.AllowGet);
        }
    }
}