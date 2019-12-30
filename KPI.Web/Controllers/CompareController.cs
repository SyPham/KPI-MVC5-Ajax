using KPI.Model.DAO;
using KPI.Model.helpers;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class CompareController : BaseController
    {
        // GET: Compare
        [BreadCrumb(Clear = true)]
        public ActionResult Index(string obj)
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.Add("/KPI/Index", "KPI");
            BreadCrumb.SetLabel("Compare");
            if (obj == null)
                return View();
            var value = obj.Split(';')[1].Split(',');
            var standard = value[0].ToInt();
            var unit = value[1].ToString();
            var comp = obj.Split(';')[0].ToString();

          
            var chartVM2s = new DataChartDAO().Compare2(comp);
            //var compare2 = new DataChartDAO().Compare2(comp);

            ViewBag.ChartVM2s = chartVM2s;
            if (chartVM2s[0].period == "W") { ViewBag.PeriodText = "Weekly"; ViewBag.Period = chartVM2s[0].period; };
            if (chartVM2s[0].period == "M") { ViewBag.PeriodText = "Monthly"; ViewBag.Period = chartVM2s[0].period; }
            if (chartVM2s[0].period == "Q") { ViewBag.PeriodText = "Quarterly"; ViewBag.Period = chartVM2s[0].period; }
            if (chartVM2s[0].period == "Y") { ViewBag.PeriodText = "Yearly"; ViewBag.Period = chartVM2s[0].period; }
            ViewBag.Standard = standard;
            ViewBag.Unit = unit;

            return View();
        }
    }
}