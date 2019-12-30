using KPI.Model.DAO;
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
    public class AddUserToLevelController : BaseController
    {
        // GET: AddUserToLevel
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("Add User Of List Each Levels");
            return View();
        }
        public async Task<ActionResult> AddUserToLevel(int id, int levelid)
        {
            return Json(await new UserAdminDAO().AddUserToLevel(id, levelid), JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> LoadDataUser(int levelid,string code,int page, int pageSize)
        {
            return Json(await new UserAdminDAO().LoadDataUser(levelid,code, page, pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}