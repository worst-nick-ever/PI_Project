using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities.Personal;
using PersonalWebsite.Database;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    public class InterestsController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? personId)
        {
            if (personId == null)
            {
                personId = db.SiteSettings.FirstOrDefault()?.PersonId;
            }

            if (personId != null)
            {
                Person person = db.People.Find(personId);
                if (person != null)
                {
                    ViewBag.PersonName = person.FirstName + " " + person.LastName;
                    return View(person.Interests ?? Enumerable.Empty<Interest>());
                }
            }
            return View(Enumerable.Empty<Interest>());
        }

        // GET: Admin/Interests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // GET: Admin/Interests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Interests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Desctiption,VideoURL")] Interest interest, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    interest.Photo = SavePhoto(photo);
                }
                Interest created = db.Interests.Add(interest);
                db.SaveChanges();

                Person selectedPerson = db.SiteSettings.Include("Person").FirstOrDefault().Person;
                selectedPerson.Interests.Add(created);
                db.Entry(selectedPerson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = selectedPerson.Id });
            }

            return View(interest);
        }

        // GET: Admin/Interests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // POST: Admin/Interests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Desctiption,VideoURL")] Interest interest, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                string filename = SavePhoto(Photo);
                if (filename != null)
                {
                    interest.Photo = filename;
                }
                db.Entry(interest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interest);
        }

        // GET: Admin/Interests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // POST: Admin/Interests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interest interest = db.Interests.Find(id);
            db.Interests.Remove(interest);
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
