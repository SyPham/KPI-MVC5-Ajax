using KPI.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBreadCrumbs;
using System.Threading.Tasks;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class KPIController : BaseController
    {
        // GET: KPI
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.Add("/KPI/Index", "KPI");
            BreadCrumb.SetLabel("Period");
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }
        public async Task<ActionResult> Period(string kpilevelcode, string period)
        {
            return Json(await new KPILevelDAO().ListDatas(kpilevelcode, period), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListTreeClient(int id)
        {
            return Json(new LevelDAO().GetListTreeClient(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListTree()
        {
            return Json(new LevelDAO().GetListTree(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataKPILevel(int level, int category, int page, int pageSize)
        {
            return Json(new KPILevelDAO().LoadDataForUser(level, category, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategoryCode(Model.EF.Category entity)
        {
            return Json(new KPILevelDAO().GetAllCategory(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateKPILevel(Model.ViewModel.UpdateKPILevelVM entity)
        {
            
            return Json(new KPILevelDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult AddComment(Model.EF.Comment entity)
        //{
        //    var value = entity.KPILevelCode;
        //    entity.KPILevelCode = value.Substring(0, value.Length - 1);
        //    entity.Period = value.Substring(value.Length - 1, 1).ToUpper();
        //    return Json(new KPILevelDAO().AddComment(entity), JsonRequestBehavior.AllowGet);
        //}

        public JsonResult LoadDataComment(int dataid,int userid)
        {
            return Json(new KPILevelDAO().ListComments(dataid,userid), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddFavourite(Model.EF.Favourite entity)
        {
            return Json(new FavouriteDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllKPILevel(int category, int page, int pageSize)
        {
            return Json(new KPILevelDAO().LoadKPILevel(category, page, pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}