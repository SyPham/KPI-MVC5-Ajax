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
    public class ListUserLevelController : BaseController
    {
        // GET: ListUserLevel
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("User Of List Each Levels");
            return View();
        }
        public async Task<ActionResult> LoadDataUser(int teamid, string code, int page, int pageSize)
        {
            return Json(await new UserAdminDAO().LoadDataUser(teamid, code, page, pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}