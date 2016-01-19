using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PersonalWebsite.Validation;
using DatabaseEntities.CV;
using PersonalWebsite.Models;

namespace PersonalWebsite.Areas.Admin.Models
{
    public class WorkEntryViewModel
    {
        public int Id { get; set; }
        [Required]
        public int CVId { get; set; }
        [Required]
        public int WorkplaceId { get; set; }
        public WorkplaceViewModel Workplace { get; set; }
        [Required]
        [DisplayName("From Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DateBefore("ToDate")]
        public DateTime FromDate { get; set; }
        [Required]
        [DisplayName("To Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DateAfter("FromDate")]
        public DateTime ToDate { get; set; }
        [Required,
         MaxLength(50, ErrorMessage = "Position must be less than 50 characters")]
        public string Position { get; set; }
        [Required]
        public string Responsibilities { get; set; }

        public WorkEntry ToWorkEntry()
        {
            return new WorkEntry
            {
                CVId = this.CVId,
                WorkplaceId = this.WorkplaceId,
                FromDate = this.FromDate,
                ToDate = this.ToDate,
                Position = this.Position,
                Responsibilities = this.Responsibilities,
                Workplace = new Workplace
                {
                    Id = this.WorkplaceId,
                    Name = this.Workplace.Name
                }
            };
        }
    }
}