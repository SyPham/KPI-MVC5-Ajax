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
    public class OCCategoriesController : BaseController
    {
        OCCategoryDAO _dao;
        public OCCategoriesController()
        {
            _dao = new OCCategoryDAO();
        }

        // GET: CategoryKPILevelAdmin
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("OC Category");
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
        public async Task<JsonResult> AddOCCategory(int OCID, int CategoryID)
        {
            return Json(await _dao.AddOCCategory(OCID, CategoryID), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetCategoryByOC(int page, int pageSize, int level, int ocID)
        {
            return Json(await _dao.GetCategoryByOC(page, pageSize, level, ocID), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetListTree()
        {
            return Json(await _dao.GetListTree(), JsonRequestBehavior.AllowGet);
        }

    }
}