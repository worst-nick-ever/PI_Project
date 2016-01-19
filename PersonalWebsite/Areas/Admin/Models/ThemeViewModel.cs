using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Admin.Models
{
    public class ThemeViewModel
    {
        public int Id { get; set; }
        [Required, MaxLength(20, ErrorMessage = "Name must be less than 20 cahracters.")]
        public string Name { get; set; }
    }
}