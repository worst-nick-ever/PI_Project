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
using PersonalWebsite.Themes;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    public class SiteSettingsController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/SiteSettings
        public ActionResult Index()
        {
            var siteSettings = db.SiteSettings.Include(s => s.CV).Include(s => s.Person).Include(s => s.Theme);
            return View(siteSettings.ToList());
        }

        // GET: Admin/SiteSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteSettings siteSettings = db.SiteSettings.Find(id);
            if (siteSettings == null)
            {
                return HttpNotFound();
            }
            return View(siteSettings);
        }

        // GET: Admin/SiteSettings/Create
        public ActionResult Create()
        {
            ViewBag.CVId = new SelectList(db.CVs, "Id", "Id");
            ViewBag.PersonId = new SelectList(db.People, "Id", "FirstName");
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Name");
            return View();
        }

        // POST: Admin/SiteSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CVId,PersonId,ThemeId")] SiteSettings siteSettings)
        {
            if (ModelState.IsValid)
            {
                db.SiteSettings.Add(siteSettings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CVId = new SelectList(db.CVs, "Id", "Id", siteSettings.CVId);
            ViewBag.PersonId = new SelectList(db.People, "Id", "FirstName", siteSettings.PersonId);
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Name", siteSettings.ThemeId);
            return View(siteSettings);
        }

        // GET: Admin/SiteSettings/Edit
        public ActionResult Edit()
        {

            ViewBag.CVList = db.CVs.Select(m => new SelectListItem
            {
                Text = m.Id + " - " + m.Person.FirstName + " " + m.Person.LastName,
                Value = m.Id.ToString()
            });
            SiteSettings siteSettings = db.SiteSettings.FirstOrDefault();
            if (siteSettings == null)
            {
                siteSettings = new SiteSettings();
                db.SiteSettings.Add(siteSettings);
                db.SaveChanges();
            }
            ViewBag.CVId = new SelectList(db.CVs, "Id", "Id", siteSettings.CVId);
            ViewBag.PersonId = new SelectList(db.People, "Id", "FirstName", siteSettings.PersonId);
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Name", siteSettings.ThemeId);
            return View(siteSettings);
        }

        // POST: Admin/SiteSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CVId,PersonId,ThemeId")] SiteSettings siteSettings)
        {

            ViewBag.CVList = db.CVs.Select(m => new SelectListItem
            {
                Text = m.Id + " - " + m.Person.FirstName + " " + m.Person.LastName,
                Value = m.Id.ToString()
            });
            if (ModelState.IsValid)
            {
                db.Entry(siteSettings).State = EntityState.Modified;
                db.SaveChanges();
                if (siteSettings.ThemeId != 0)
                {
                    Theme t = db.Themes.Find(siteSettings.ThemeId);
                    if (t != null)
                    {
                        ThemeContainer.SetCurrentTheme(t.URL);
                    }
                }
                return RedirectToAction("CV", new { area = "", controller = "Guest" });
            }
            ViewBag.CVId = new SelectList(db.CVs, "Id", "Id", siteSettings.CVId);
            ViewBag.PersonId = new SelectList(db.People, "Id", "FirstName", siteSettings.PersonId);
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Name", siteSettings.ThemeId);
            return View(siteSettings);
        }

        // GET: Admin/SiteSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteSettings siteSettings = db.SiteSettings.Find(id);
            if (siteSettings == null)
            {
                return HttpNotFound();
            }
            return View(siteSettings);
        }

        // POST: Admin/SiteSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteSettings siteSettings = db.SiteSettings.Find(id);
            db.SiteSettings.Remove(siteSettings);
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
