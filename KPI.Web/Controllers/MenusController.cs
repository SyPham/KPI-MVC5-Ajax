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
using KPI.Web.Models.MenuViewModel;

namespace KPI.Web.Controllers
{
    public class MenusController : BaseController
    {
        private KPIDbContext db = new KPIDbContext();

        // GET: Menus
        public async Task<ActionResult> Index()
        {
            var model = await db.Menus.Select(x => new MenuVM
            {
                ID = x.ID,
                Name = x.Name,
                FontAwesome = x.FontAwesome,
                BackgroudColor = x.BackgroudColor,
                Link = x.Link,
                Position = x.Position,
                PermissionName = db.Permissions.FirstOrDefault(a=>a.ID == x.Permission).PermissionName
            }).ToListAsync();
            var user = (UserProfileVM)Session["UserProfile"];
            if (user.User.Permission == 1)
            {
                return View(model);
            }
            else
            {
                return Redirect("~/Error/NotFound");
            }
        }

        // GET: Menus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = await db.Menus.FindAsync(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            ViewBag.Permission = db.Permissions;
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuViewModel menuvm)
        {
            if (ModelState.IsValid)
            {
                var menu = new Menu
                {
                    ID = menuvm.ID,
                    Name = menuvm.LangNameEn,
                    BackgroudColor = menuvm.BackgroudColor,
                    FontAwesome=menuvm.FontAwesome,
                    Permission = menuvm.Permission,
                    Position=menuvm.Position,
                    Link = menuvm.Link
                };
               
                db.Menus.Add(menu);
                await db.SaveChangesAsync();
                var listMenuLang = new List<MenuLang>();
                var menulang = new MenuLang
                {
                    MenuID = menu.ID,
                    Name = menuvm.LangNameEn,
                    LangID="en"
                };
                listMenuLang.Add(menulang);
                var menulangVi = new MenuLang
                {
                    MenuID = menu.ID,
                    Name = menuvm.LangNameVi,
                    LangID = "vi"
                };
                listMenuLang.Add(menulangVi);

                var menulangTw = new MenuLang
                {
                    MenuID = menu.ID,
                    Name = menuvm.LangNameTw,
                    LangID = "tw"
                };
                listMenuLang.Add(menulangTw);

                db.MenuLangs.AddRange(listMenuLang);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(menuvm);
        }

        // GET: Menus/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = await db.Menus.FindAsync(id);

            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.Permission = db.Permissions;

            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Permission = db.Permissions;

            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = await db.Menus.FindAsync(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Menu menu = await db.Menus.FindAsync(id);
            db.Menus.Remove(menu);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
