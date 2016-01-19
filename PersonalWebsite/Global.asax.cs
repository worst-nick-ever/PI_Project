using DatabaseEntities.Settings;
using PersonalWebsite.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PersonalWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SetSiteTheme();
        }

        private static void SetSiteTheme()
        {
            Database.ApplicationDbContext db = new Database.ApplicationDbContext();
            SiteSettings settings = db.SiteSettings.FirstOrDefault(x => x.ThemeId != 0);
            if (settings != null)
            {
                Theme theme = db.Themes.Find(settings.ThemeId);
                if (theme != null)
                {
                    ThemeContainer.SetCurrentTheme(theme.URL);
                }
            }
        }
    }
}
