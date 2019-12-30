using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KPI.Model;
using KPI.Model.EF;
using KPI.Model.ViewModel;

namespace KPI.Web.Controllers
{
    public class SettingsController : BaseController
    {
        private KPIDbContext db = new KPIDbContext();

        // GET: Settings
        public async Task<ActionResult> Index()
        {
            var user = (UserProfileVM)Session["UserProfile"];
            if (user.User.Permission == 1)
            {
                return View(await db.Settings.ToListAsync());
            }
            else
            {
                return Redirect("~/Error/NotFound");
            }
        }

        // GET: Settings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = await db.Settings.FindAsync(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Settings.Add(setting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(setting);
        }

        // GET: Settings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = await db.Settings.FindAsync(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Code,Name,State,CreatedTime")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        // GET: Settings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = await db.Settings.FindAsync(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Setting setting = await db.Settings.FindAsync(id);
            db.Settings.Remove(setting);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<JsonResult> ShowInfo()
        {
            Setting setting = await db.Settings.FirstOrDefaultAsync(x => x.State == true && x.Code == "INFO");
            if(setting != null)
            return Json(new { setting.Name, setting.State }, JsonRequestBehavior.AllowGet);
            return Json(new { State = false }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Maintain()
        {
            Setting setting = await db.Settings.FirstOrDefaultAsync(x => x.State == true && x.Code == "MAINTAIN");
            if (setting != null)
                return Json(new { setting.Name, setting.State }, JsonRequestBehavior.AllowGet);
            return Json(new { State = false }, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
