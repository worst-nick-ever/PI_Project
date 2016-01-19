using DatabaseEntities.CV;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Areas.Admin.Models
{
    public class CVViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Person")]
        public int PersonId { get; set; }
        public ICollection<WorkEntryViewModel> WorkEntries { get; set; }
    }
}