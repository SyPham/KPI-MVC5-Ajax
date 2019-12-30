using KPI.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    public class ActionPlanController : BaseController
    {
        // GET: ActionPlan
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> LoadData(int? catid)
        {

            return Json(await new ActionPlanDAO().GetActionPlanByCategory(catid ?? 0), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetAllCategory()
        {

            return Json(await new AdminCategoryDAO().GetAll(), JsonRequestBehavior.AllowGet);
        }
    }
}