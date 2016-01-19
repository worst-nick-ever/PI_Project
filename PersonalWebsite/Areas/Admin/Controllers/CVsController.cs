using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities.CV;
using PersonalWebsite.Database;
using System.Data.Entity.Infrastructure;
using PersonalWebsite.Areas.Admin.Models;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    public class CVsController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/CVs
        public ActionResult Index()
        {
            return View(db.CVs.Include("Person").ToList());
        }

        // GET: Admin/CVs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = db.CVs.Find(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            return View(cV);
        }

        // GET: Admin/CVs/Create
        public ActionResult Create()
        {
            AddPeopleListToView();
            return View();
        }

        // POST: Admin/CVs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,WorkEntries")] CVViewModel cv)
        {
            if (ModelState.IsValid)
            {
                CV databaseEntry = new CV();
                databaseEntry.PersonId = cv.PersonId;
                databaseEntry.WorkEntries = new List<WorkEntry>();
                foreach (WorkEntryViewModel wevm in cv.WorkEntries)
                {
                    databaseEntry.WorkEntries.Add(wevm.ToWorkEntry());
                }
                db.CVs.Add(databaseEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            AddPeopleListToView();
            return View(cv);
        }

        // GET: Admin/CVs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cv = db.CVs.Include("WorkEntries.Workplace").Include("Person").Single(c => c.Id == id);
            if (cv == null)
            {
                return HttpNotFound();
            }
            AddPeopleListToView();
            CVViewModel cvViewModel = new CVViewModel
            {
                Id = cv.Id,
                PersonId = cv.PersonId,
                WorkEntries = new List<WorkEntryViewModel>()
            };
            foreach (WorkEntry we in cv.WorkEntries)
            {
                cvViewModel.WorkEntries.Add(new WorkEntryViewModel
                {
                    Id = we.Id,
                    CVId = we.CVId,
                    WorkplaceId = we.WorkplaceId,
                    Workplace = new WorkplaceViewModel
                    {
                        Id = we.WorkplaceId,
                        Name = we.Workplace.Name
                    },
                    FromDate = we.FromDate,
                    ToDate = we.ToDate,
                    Position = we.Position,
                    Responsibilities = we.Responsibilities
                });
            }
            AddPeopleListToView();
            return View(cvViewModel);
        }

        // POST: Admin/CVs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PersonId,WorkEntries")] CVViewModel cv)
        {
            if (ModelState.IsValid)
            {
                CV databaseEntity = db.CVs.FirstOrDefault(x => x.Id == cv.Id);
                foreach (WorkEntryViewModel we in cv.WorkEntries)
                {
                    Workplace found = db.Workplaces.FirstOrDefault(wp => wp.Name.Equals(we.Workplace.Name));
                    if (found != null)
                    {
                        we.WorkplaceId = found.Id;
                    }
                    else
                    {
                        Workplace created = db.Workplaces.Add(we.Workplace.ToWorkplace());
                        we.WorkplaceId = created.Id;
                    }

                    if (we.Id != 0)
                    {
                        WorkEntry exisitng = db.WorkEntries.Find(we.Id);
                        exisitng.Position = we.Position;
                        exisitng.Responsibilities = we.Responsibilities;
                        exisitng.FromDate = we.FromDate;
                        exisitng.ToDate = we.ToDate;
                        exisitng.WorkplaceId = we.WorkplaceId;
                        exisitng.CVId = we.CVId;
                        db.Entry(exisitng).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        we.CVId = cv.Id;
                        WorkEntry created = db.WorkEntries.Add(we.ToWorkEntry());
                        databaseEntity.WorkEntries.Add(created);
                    }
                }
                List<WorkEntry> inDatabase = db.WorkEntries.Where(x => x.CVId == cv.Id).ToList();
                foreach (WorkEntry workEntry in inDatabase)
                {
                    if (!cv.WorkEntries.Any(x => x.Id == workEntry.Id))
                    {
                        db.WorkEntries.Remove(workEntry);
                    }
                }
                db.Entry(databaseEntity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            AddPeopleListToView();
            return View(cv);
        }

        private void AddPeopleListToView()
        {
            ViewBag.PeopleList = db.People.Select(m => new SelectListItem
            {
                Text = m.FirstName + " " + m.LastName,
                Value = m.Id.ToString()
            });
        }

        // GET: Admin/CVs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = db.CVs.Find(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            return View(cV);
        }

        // POST: Admin/CVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CV cV = db.CVs.Find(id);
            db.CVs.Remove(cV);
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
