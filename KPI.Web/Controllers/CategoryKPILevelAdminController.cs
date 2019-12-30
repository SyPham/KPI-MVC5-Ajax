using KPI.Model.DAO;
using KPI.Model.EF;
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
    public class CategoryKPILevelAdminController : BaseController
    {
        private CategoryKPILevelAdminDAO _dao = null;
        public CategoryKPILevelAdminController()
        {
            _dao = new CategoryKPILevelAdminDAO();
        }

        // GET: CategoryKPILevelAdmin
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("OC Category KPI");
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

        public async Task<JsonResult> GetAllCategories(int page, int pageSize, int level,int OCID)
        {
            return Json(await _dao.GetCategoryByOC(page, pageSize, level, OCID), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAllKPIlevels(int page, int pageSize)
        {
            return Json(await _dao.GetAllKPIlevels(page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> LoadDataKPILevel(int level, int category, int page, int pageSize)
        {
            return Json(await _dao.LoadDataKPILevel(level, category, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Add(CategoryKPILevel entity)
        {
            return Json(await _dao.Add(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AddGeneral(int kpilevel, int category, string pic, string owner, string manager, string sponsor, string participant)
        {
            return Json(await _dao.AddGeneral(kpilevel, category, pic, owner, manager, sponsor, participant), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetUserByCategoryIDAndKPILevelID(int KPILevelID, int CategoryID)
        {
            return Json(await _dao.GetUserByCategoryIDAndKPILevelID(CategoryID, KPILevelID), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> RemoveCategoryKPILevel(int KPILevelID, int CategoryID)
        {
            return Json(await _dao.RemoveCategoryKPILevel(CategoryID, KPILevelID), JsonRequestBehavior.AllowGet);
        }
        
    }
}