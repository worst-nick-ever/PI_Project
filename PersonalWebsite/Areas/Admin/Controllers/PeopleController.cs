using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities.Personal;
using PersonalWebsite.Database;
using PersonalWebsite.Areas.Admin.Models;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    public class PeopleController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/People
        public ActionResult Index()
        {
            return View(db.People.ToList());
        }

        // GET: Admin/People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Admin/People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,MiddleName,LastName,BirthDate,Gender,Address,Phone,Email,Skype,Facebook,Linkedin,OtherAbilities,ForeignLanguages")] PersonViewModel personViewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                Person person = GetPersonFromViewModel(personViewModel);
                if (upload != null)
                {
                    person.PhotoFileName = SavePhoto(upload);
                }
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personViewModel);
        }

        // GET: Admin/People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.
                Include("Contacts").
                Include("ForeignLanguages").
                Include("OtherAbilities").
                FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(GetViewModelFormPerson(person));
        }

        // POST: Admin/People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,MiddleName,LastName,BirthDate,Gender,ContactsId,Address,Phone,Email,Skype,Facebook,Linkedin,OtherAbilities,ForeignLanguages")] PersonViewModel personViewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                Person person = db.People.
                    Include("Contacts").
                    Include("ForeignLanguages").
                    FirstOrDefault(p => p.Id == personViewModel.Id);
                person.FirstName = personViewModel.FirstName;
                person.MiddleName = personViewModel.MiddleName;
                person.LastName = personViewModel.LastName;
                person.BirthDate = personViewModel.BirthDate;
                person.Gender = personViewModel.Gender;
                person.ContactsId = personViewModel.ContactsId;
                person.Contacts.Address = personViewModel.Address;
                person.Contacts.Phone = personViewModel.Phone;
                person.Contacts.Email = personViewModel.Email;
                person.Contacts.Skype = personViewModel.Skype;
                person.Contacts.Facebook = personViewModel.Facebook;
                person.Contacts.Linkedin = personViewModel.Linkedin;

                if (personViewModel.ForeignLanguages != null && personViewModel.ForeignLanguages.Count > 0)
                {
                    foreach (ForeignLanguage fl in personViewModel.ForeignLanguages)
                    {
                        if (person.ForeignLanguages.Contains(fl))
                        {
                            continue;
                        }
                        ForeignLanguage found = db.ForeignLanguages.FirstOrDefault(f => f.Name.Equals(fl.Name, System.StringComparison.InvariantCultureIgnoreCase));
                        if (found != null)
                        {
                            person.ForeignLanguages.Add(found);
                        }
                        else
                        {
                            ForeignLanguage created = db.ForeignLanguages.Add(fl);
                            person.ForeignLanguages.Add(created);
                        }
                    }
                }
                else
                {
                    person.ForeignLanguages.Clear();
                }
                
                foreach (ForeignLanguage fl in person.ForeignLanguages.Where(x => !personViewModel.ForeignLanguages.Contains(x)).ToList())
                {
                    person.ForeignLanguages.Remove(fl);
                }

                if (person.OtherAbilities != null)
                {
                    foreach (Ability ability in personViewModel.OtherAbilities)
                    {
                        if (ability.Id != 0)
                        {
                            //db.Abilities.Attach(ability);
                            Ability exisitng = db.Abilities.Find(ability.Id);
                            exisitng.Name = ability.Name;
                            exisitng.Description = ability.Description;
                            db.Entry(exisitng).State = EntityState.Modified;
                        }
                        else
                        {
                            ability.PersonId = person.Id;
                            Ability created = db.Abilities.Add(ability);
                            ability.Id = created.Id;
                        }
                    }
                }
                List<Ability> oldAbilities = db.Abilities.Where(a => a.PersonId == personViewModel.Id).ToList();
                oldAbilities.RemoveAll(x => personViewModel.OtherAbilities.Contains(x));
                db.Abilities.RemoveRange(oldAbilities);

                if (upload != null)
                {
                    person.PhotoFileName = SavePhoto(upload);
                }

                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personViewModel);
        }

        private Person GetPersonFromViewModel(PersonViewModel personViewModel)
        {
            return new Person
            {
                Id = personViewModel.Id ?? 0,
                FirstName = personViewModel.FirstName,
                MiddleName = personViewModel.MiddleName,
                LastName = personViewModel.LastName,
                BirthDate = personViewModel.BirthDate,
                Gender = personViewModel.Gender,
                ContactsId = personViewModel.ContactsId,
                Contacts = new Contacts
                {
                    Id = personViewModel.ContactsId,
                    Address = personViewModel.Address,
                    Phone = personViewModel.Phone,
                    Email = personViewModel.Email,
                    Skype = personViewModel.Skype,
                    Facebook = personViewModel.Facebook,
                    Linkedin = personViewModel.Linkedin
                },
                OtherAbilities = personViewModel.OtherAbilities,
                ForeignLanguages = personViewModel.ForeignLanguages
            };
        }

        private PersonViewModel GetViewModelFormPerson(Person person)
        {
            return new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                BirthDate = person.BirthDate,
                Gender = person.Gender,
                ContactsId = person.ContactsId,
                Address = person.Contacts.Address,
                Phone = person.Contacts.Phone,
                Email = person.Contacts.Email,
                Skype = person.Contacts.Skype,
                Facebook = person.Contacts.Facebook,
                Linkedin = person.Contacts.Linkedin,
                PhotoFileName = person.PhotoFileName,
                OtherAbilities = person.OtherAbilities,
                ForeignLanguages = person.ForeignLanguages
            };
        }

        // GET: Admin/People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Admin/People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
