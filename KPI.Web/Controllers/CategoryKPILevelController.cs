using KPI.Model.DAO;
using KPI.Model.EF;
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
    public class CategoryKPILevelController : BaseController
    {
        private AdminCategoryDAO _adminCategorydao = null;
        private KPILevelDAO _kpiLeveldao = null;
        private CategoryKPILevelAdminDAO _dategoryKPILevelAdmindao = null;
        private CategoryKPILevelDAO _dategoryKPILeveldao = null;
        public CategoryKPILevelController()
        {
            _adminCategorydao = new AdminCategoryDAO();
            _kpiLeveldao = new KPILevelDAO();
            _dategoryKPILevelAdmindao = new CategoryKPILevelAdminDAO();
            _dategoryKPILeveldao = new CategoryKPILevelDAO();

        }

        // GET: CategoryKPILevelAdmin
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("KPI");
            return View();
        }

        public async Task<JsonResult> GetAllCategories(int page, int pageSize, int level,int ocID)
        {
            return Json(await _adminCategorydao.GetAllCategory(page, pageSize, level, ocID),JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAllKPIlevels(int page, int pageSize)
        {
            return Json(await _kpiLeveldao.GetAll(page, pageSize), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Add(CategoryKPILevel entity)
        {
            return Json(await _dategoryKPILevelAdmindao.Add(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllKPILevelByCategory(int category, int page, int pageSize)
        {
            return Json( _dategoryKPILeveldao.LoadKPILevel(category, page, pageSize), JsonRequestBehavior.AllowGet);

        }
    }
}