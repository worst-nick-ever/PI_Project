using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Themes
{
    public class ThemeContainer
    {
        public const string DefaultTheme = "~/Content/bootstrap.min.css";

        public static string CurrentTheme = DefaultTheme;

        public static void SetCurrentTheme(string filename)
        {
            CurrentTheme = "~/Content/Bootstrap/" + filename;
        }
    }
}