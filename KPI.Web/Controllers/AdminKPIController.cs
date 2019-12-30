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
    [BreadCrumb(Clear =true)]
    public class AdminKPIController : BaseController
    {
        // GET: AdminKPI
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("KPI");
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
        public ActionResult ListKPI()
        {
            return View();
        }
        public async Task<JsonResult> Add(Model.EF.KPI entity)
        {
            return  Json(await new KPIAdminDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> ListCategory(Model.EF.Category entity)
        {
            return  Json(await new KPIAdminDAO().ListCategory(), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AddKPILevel(Model.EF.KPILevel entity)
        {
            return  Json(await new KPIAdminDAO().AddKPILevel(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetAll()
        {
            return  Json(await new KPIAdminDAO().GetAll(), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Delete(int id)
        {
            return  Json(await new KPIAdminDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Update(Model.EF.KPI entity)
        {
            return  Json(await new KPIAdminDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetbyID(int ID)
        {
            return  Json(await new KPIAdminDAO().GetbyID(ID), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> LoadData(int? catID,string name, int page, int pageSize)
        {
            return  Json(await new KPIAdminDAO().LoadData(catID,name,page,pageSize), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Autocomplete(string name)
        {
            return  Json(await new KPIAdminDAO().Autocomplete(name), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetAllUnit()
        {
            return  Json(await new KPIAdminDAO().GetAllUnit(), JsonRequestBehavior.AllowGet);
        }
    }
}