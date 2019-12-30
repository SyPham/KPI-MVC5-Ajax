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
    public class AdminUserController : BaseController
    {
        private UserAdminDAO _dao = null;
        private KPILevelDAO _kpileveldao = null;
        public AdminUserController()
        {
            _dao = new UserAdminDAO();
            _kpileveldao = new KPILevelDAO();
        }

        // GET: Account
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("User");
            var user = (UserProfileVM)Session["UserProfile"] ;
            if(user.User.Permission == 1)
            {
                return View();
            }
            else
            {
                return Redirect("~/Error/NotFound");
            }
        }
        public async Task<JsonResult> Add(Model.EF.User entity)
        {
            return Json(await _dao.Add(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetAll()
        {
            return Json(await _dao.GetAll(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> LoadData(string search, int page, int pageSize)
        {
            return Json(await _dao.LoadData(search, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Delete(int id)
        {
            return Json(await _dao.Delete(id), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Update(Model.EF.User entity)
        {
            return Json(await _dao.Update(entity), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetbyID(int ID)
        {
            return Json(await _dao.GetbyID(ID), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> LockUser(int ID)
        {
            return Json(await _dao.LockUser(ID), JsonRequestBehavior.AllowGet);
        }
        //update kpiLevel
        public async Task<JsonResult> UpdateKPILevel(Model.ViewModel.UpdateKPILevelVM entity)
        {
            return Json(await _kpileveldao.Update(entity), JsonRequestBehavior.AllowGet);
        }
        //get all kpilevel
        public async Task<JsonResult> GetAllKPILevel()
        {
            return Json(await _kpileveldao.GetAll(), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetListAllPermissions(int userid)
        {
            return Json(await _dao.GetListAllPermissions(userid), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetAllMenus()
        {
            return Json(await _dao.GetAllMenus(), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Checkpermisson(int userid)
        {
            return Json(await _dao.Checkpermisson(userid), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> ChangePassword(string username,string password)
        {
            return Json(await _dao.ChangePassword(username, password), JsonRequestBehavior.AllowGet);
        }
        
    }
}