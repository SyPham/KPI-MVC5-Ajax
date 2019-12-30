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
    public class DatasetController : BaseController
    {
        private DataChartDAO _dao = null;
        public DatasetController()
        {
            _dao = new DataChartDAO();
        }

        // GET: Dataset
        [BreadCrumb(Clear = true)]
        public async Task<ActionResult> Index(int catid, string period, int? start = 0, int? end = 0, int? year = 0)
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.Add("/Dataset/Index", "Datasets");
            var datasets =await _dao.GetAllDataByCategory(catid, period, start, end, year);
            
            ViewBag.Datasets = datasets;
            return View();
        }
    }
}