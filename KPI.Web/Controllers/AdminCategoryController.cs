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
    public class AdminCategoryController : BaseController
    {
        // GET: Category
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("Category");
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
        public  async Task<JsonResult> Add(Model.EF.Category entity)
        {
            return Json(await new AdminCategoryDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public  async Task<JsonResult> Update(Model.EF.Category entity)
        {
            return Json(await new AdminCategoryDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        public  async Task<JsonResult> ListCategory(Model.EF.Category entity)
        {
            return Json(await new AdminCategoryDAO().ListCategory(), JsonRequestBehavior.AllowGet);
        }
       
        public  async Task<JsonResult> GetAll()
        {
            return Json(await new AdminCategoryDAO().GetAll(), JsonRequestBehavior.AllowGet);
        }
        public  async Task<JsonResult> Delete(int id)
        {
            return Json(await new AdminCategoryDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }
        
        public  async Task<JsonResult> GetbyID(int ID)
        {
            return Json(await new AdminCategoryDAO().GetbyID(ID), JsonRequestBehavior.AllowGet);
        }
        public  async Task<JsonResult> LoadData( string name, int page, int pageSize)
        {
            return Json(await new AdminCategoryDAO().LoadData( name, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public  async Task<JsonResult> Autocomplete(string name)
        {
            return Json(await new AdminCategoryDAO().Autocomplete(name), JsonRequestBehavior.AllowGet);
        }
    }
}