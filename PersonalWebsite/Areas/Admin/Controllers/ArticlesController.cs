using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DatabaseEntities.Others;
using PersonalWebsite.Database;
using DatabaseEntities.Personal;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    public class ArticlesController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Articles
        public ActionResult Index(int? authorId)
        {
            if (authorId == null)
            {
                authorId = db.SiteSettings.FirstOrDefault()?.PersonId;
            }

            if (authorId != null)
            {
                IEnumerable<Article> articles = db.Articles.Where(a => a.AuthorId == (int)authorId);
                if (articles != null)
                {
                    Person author = db.People.FirstOrDefault(p => p.Id == (int)authorId);
                    ViewBag.PersonName = author.FirstName + " " + author.LastName;
                    return View(articles);
                }
            }
            return View(Enumerable.Empty<Article>());
        }

        // GET: Admin/Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Include("Author").FirstOrDefault(x => x.Id == id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Admin/Articles/Create
        public ActionResult Create()
        {
            ViewBag.People = db.People.Select(m => new SelectListItem
            {
                Text = m.FirstName + " " + m.LastName,
                Value = m.Id.ToString()
            });
            return View();
        }

        // POST: Admin/Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorId,Title,Paragraphs")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.People = db.People.Select(m => new SelectListItem
            {
                Text = m.FirstName + " " + m.LastName,
                Value = m.Id.ToString()
            });
            return View(article);
        }

        // GET: Admin/Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.People = db.People.Select(m => new SelectListItem
            {
                Text = m.FirstName + " " + m.LastName,
                Value = m.Id.ToString()
            });
            return View(article);
        }

        // POST: Admin/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AuthorId,Title,Paragraphs")] Article article)
        {
            if (ModelState.IsValid)
            {
                foreach (Paragraph p in article.Paragraphs)
                {
                    if (p.Id != 0)
                    {
                        db.Paragraphs.Attach(p);
                        db.Entry(p).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        p.ArticleId = article.Id;
                        Paragraph created = db.Paragraphs.Add(p);
                        db.Paragraphs.Add(created);
                        db.SaveChanges();
                    }
                }
                List<Paragraph> oldParagraphs = db.Paragraphs.Where(p => p.ArticleId == article.Id).ToList();
                oldParagraphs.RemoveAll(article.Paragraphs.Contains);
                db.Paragraphs.RemoveRange(oldParagraphs);
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Admin/Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Admin/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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
