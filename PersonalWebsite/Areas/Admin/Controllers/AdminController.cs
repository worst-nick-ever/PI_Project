using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalWebsite.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        protected String SavePhoto(HttpPostedFileBase upload)
        {
            if (IsValidPhoto(upload))
            {
                return SaveFile(upload, "~/Content/Uploads");
            }
            return null;
        }

        private bool IsValidPhoto(HttpPostedFileBase upload)
        {
            //check if larger than 2Mb
            if (upload == null || upload.ContentLength > 2 * 1024 * 1024)
            {
                return false;
            }

            //check file type
            try
            {
                var allowedFormats = new[]
                {
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Gif,
                ImageFormat.Bmp
            };

                using (var img = Image.FromStream(upload.InputStream))
                {
                    return allowedFormats.Contains(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }
        }

        protected String SaveFile(HttpPostedFileBase upload, string folder)
        {
            //check if larger than 200Kb
            if (upload == null || upload.ContentLength > 200 * 1024)
            {
                return null;
            }

            if (upload?.ContentLength > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(upload.FileName);
                string uploadFolder = Server.MapPath(folder);
                var path = Path.Combine(uploadFolder, fileName);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                upload.SaveAs(path);
                return fileName;
            }

            return null;
        }
    }
}