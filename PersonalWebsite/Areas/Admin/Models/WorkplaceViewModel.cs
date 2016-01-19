using DatabaseEntities.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Admin.Models
{
    public class WorkplaceViewModel
    {
        public int Id { get; set; }
        [Required,
         MaxLength(50, ErrorMessage = "Name must be less than 50 characters")]
        public string Name { get; set; }

        public Workplace ToWorkplace()
        {
            return new Workplace
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }
}