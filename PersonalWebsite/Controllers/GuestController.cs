using System.Linq;
using System.Web.Mvc;
using PersonalWebsite.Database;
using DatabaseEntities.Others;
using System.Collections.Generic;
using DatabaseEntities.Personal;
using DatabaseEntities.CV;
using DatabaseEntities.Settings;
using PersonalWebsite.Models;
using System.Net;
using DatabaseEntities.Message;
using System.Web.Helpers;
using System.Text;

namespace PersonalWebsite.Controllers
{
    public class GuestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Guest
        public ActionResult Blog()
        {
            if (db.SiteSettings.Any())
            {
                int? selectedPersonId = db.SiteSettings.FirstOrDefault().PersonId;
                if (selectedPersonId != null)
                {
                    ICollection<Article> articles = db.Articles.Include("Author").Where(a => a.Author.Id == (int)selectedPersonId).ToList();
                    if (articles != null)
                    {
                        return View(new BlogViewModel
                        {
                            AuthorId = selectedPersonId,
                            Articles = articles
                        });
                    }
                }
            }
            return View("NoData");
        }

        //GET: 
        public ActionResult Article(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Include("Author").FirstOrDefault(a => a.Id == (int)id);
            if (article != null)
            {
                return View(article);
            }
            else
            {
                return View("NoData");
            }
        }

        // GET: Guest
        public ActionResult Chat()
        {
            return View();
        }

        // GET: Guest
        public ActionResult Contacts()
        {
            if (db.SiteSettings.Any())
            {
                Contacts selectedPersonContacts = db.SiteSettings.
                    Include("Person.Contacts").First().Person.Contacts;
                if (selectedPersonContacts != null)
                {
                    return View(selectedPersonContacts);
                }
            }
            return View("NoData");
        }

        // GET: Guest
        public ActionResult CV()
        {
            if (db.SiteSettings.Any(s => s.CV != null && s.Person != null))
            {
                CV selectedCV = db.SiteSettings.
                    Include("CV.Person.Contacts").
                    Include("CV.Person.OtherAbilities").
                    Include("CV.Person.ForeignLanguages").
                    Include("CV.WorkEntries.Workplace").FirstOrDefault()?.CV;
                return View(selectedCV);
            }
            ViewBag.DataType = "CVs";
            return View("NoData");
        }

        // GET: Guest
        public ActionResult Interests()
        {
            if (db.SiteSettings.Any())
            {
                SiteSettings settings = db.SiteSettings.Include("Person").FirstOrDefault();
                Person selectedPerson = settings.Person;
                if (selectedPerson != null)
                {
                    return View(selectedPerson);
                }
            }
            return View("NoData");
        }

        // GET: Guest
        public ActionResult Places()
        {
            if (db.SiteSettings.Any())
            {
                Person selectedPerson = db.SiteSettings.Include("Person.Places").First().Person;
                if (selectedPerson != null)
                {
                    return View(selectedPerson);
                }
            }
            return View("NoData");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Message([Bind(Include = "SenderEmail,About,Text")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return Json(new { success = true, message = "Your message has been sent!" });
            }
            StringBuilder sb = new StringBuilder();
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    sb.Append(error.ErrorMessage);
                }
            }
            return Json(new { success = false, error = sb.ToString()});
        }

    }
}