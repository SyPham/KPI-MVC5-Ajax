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
    public class FavouriteController : BaseController
    {
        // GET: Favourite
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("Favourite");
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> LoadData(int userid, int page, int pageSize)
        {
            return Json(await new FavouriteDAO().LoadData(userid, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            return Json(await new FavouriteDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }

    }
}