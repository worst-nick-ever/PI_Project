using DatabaseEntities.CV;
using DatabaseEntities.Message;
using DatabaseEntities.Others;
using DatabaseEntities.Personal;
using DatabaseEntities.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            System.Data.Entity.Database.SetInitializer(new AdminUserInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<CV> CVs { get; set; }
        public virtual DbSet<WorkEntry> WorkEntries { get; set; }
        public virtual DbSet<Workplace> Workplaces { get; set; }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Paragraph> Paragraphs { get; set; }
        public virtual DbSet<Place> Places { get; set; }

        public virtual DbSet<Ability> Abilities { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<ForeignLanguage> ForeignLanguages { get; set; }
        public virtual DbSet<Interest> Interests { get; set; }
        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<SiteSettings> SiteSettings { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }

    public class AdminUserInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            CreateAdminUser(context);
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[SiteSettings] DROP CONSTRAINT [FK_dbo.SiteSettings_dbo.CVs_CVId]");
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[SiteSettings] ADD CONSTRAINT [FK_dbo.SiteSettings_dbo.CVs_CVId] FOREIGN KEY (CVId) REFERENCES [dbo].[CVs]([Id]) ON UPDATE NO ACTION ON DELETE SET NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[SiteSettings] DROP CONSTRAINT [FK_dbo.SiteSettings_dbo.People_PersonId]");
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[SiteSettings] ADD CONSTRAINT [FK_dbo.SiteSettings_dbo.People_PersonId] FOREIGN KEY (PersonId) REFERENCES [dbo].[People]([Id]) ON UPDATE NO ACTION ON DELETE SET NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[SiteSettings] DROP CONSTRAINT [FK_dbo.SiteSettings_dbo.Themes_ThemeId]");
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[SiteSettings] ADD CONSTRAINT [FK_dbo.SiteSettings_dbo.Themes_ThemeId] FOREIGN KEY (ThemeId) REFERENCES [dbo].[Themes]([Id]) ON UPDATE NO ACTION ON DELETE SET NULL");
        }

        private void CreateAdminUser(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string adminRole = "Admin";
            string userName = "admin@admin.com";
            string password = "123456";


            //Create Role Admin and User admin
            RoleManager.Create(new IdentityRole(adminRole));

            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(adminRole))
            {
                var roleresult = RoleManager.Create(new IdentityRole(adminRole));
            }

            //Create User=Admin with password=123456
            var user = new ApplicationUser { UserName = userName, Email = userName };
            var adminresult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, adminRole);
            }
        }
    }
}