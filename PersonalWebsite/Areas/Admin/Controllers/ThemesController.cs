using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities.Settings;
using PersonalWebsite.Database;
using PersonalWebsite.Areas.Admin.Models;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    public class ThemesController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Themes
        public ActionResult Index()
        {
            return View(db.Themes.ToList());
        }

        // GET: Admin/Themes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = db.Themes.Find(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // GET: Admin/Themes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ThemeViewModel theme, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null)
            {
                string filename = SaveFile(file, "~/Content/Bootstrap");
                if (filename != null)
                {
                    Theme dbTheme = new Theme
                    {
                        Id = theme.Id,
                        Name = theme.Name,
                        URL = filename
                    };
                    db.Themes.Add(dbTheme);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(theme);
        }

        // GET: Admin/Themes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = db.Themes.Find(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            ThemeViewModel tvm = new ThemeViewModel
            {
                Id = theme.Id,
                Name = theme.Name
            };
            ViewBag.Filename = theme.URL;
            return View(tvm);
        }

        // POST: Admin/Themes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ThemeViewModel theme, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string filename = SaveFile(file, "~/Content/Bootstrap");
                if (filename != null)
                {
                    Theme dbTheme = new Theme
                    {
                        Id = theme.Id,
                        Name = theme.Name,
                        URL = filename
                    };
                    db.Entry(dbTheme).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(theme);
        }

        // GET: Admin/Themes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = db.Themes.Find(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // POST: Admin/Themes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Theme theme = db.Themes.Find(id);
            db.Themes.Remove(theme);
            db.SaveChanges();
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
