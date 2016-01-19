using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities.Others;
using PersonalWebsite.Database;
using DatabaseEntities.Personal;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    public class PlacesController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Places/{id}
        public ActionResult Index(int? personId)
        {
            if (personId == null)
            {
                personId = db.SiteSettings.FirstOrDefault().PersonId;
            }

            if (personId != null)
            {
                Person person = db.People.Find(personId);
                if (person != null)
                {
                    return View(person.Places ?? Enumerable.Empty<Place>());
                }
            }
            return View(Enumerable.Empty<Place>());
        }

        // GET: Admin/Places/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // GET: Admin/Places/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Longitude,Latitude,Description")] Place place, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    place.PhotoURL = SavePhoto(photo);
                }
                Place created = db.Places.Add(place);
                db.SaveChanges();

                Person selectedPerson = db.SiteSettings.Include("Person").FirstOrDefault().Person;
                selectedPerson.Places.Add(created);
                db.Entry(selectedPerson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(place);
        }

        // GET: Admin/Places/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: Admin/Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Longitude,Latitude,Description")] Place place, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    place.PhotoURL = SavePhoto(photo);
                }
                db.Entry(place).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(place);
        }

        // GET: Admin/Places/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: Admin/Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Place place = db.Places.Find(id);
            db.Places.Remove(place);
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
