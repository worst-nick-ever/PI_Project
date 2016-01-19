using DatabaseEntities.Personal;
using PersonalWebsite.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Areas.Admin.Models
{
    public class PersonViewModel
    {
        public int? Id { get; set; }
        [Required,
         MaxLength(50, ErrorMessage = "First Name must be less than 50 characters")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required,
         MaxLength(50, ErrorMessage = "Middle Name must be less than 50 characters")]
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [Required,
         MaxLength(50, ErrorMessage = "Last Name must be less than 50 characters")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date of birth")]
        [BeforeToday]
        public DateTime BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public int ContactsId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string PhotoFileName { get; set; }
        public virtual ICollection<Ability> OtherAbilities { get; set; }
        public virtual ICollection<ForeignLanguage> ForeignLanguages { get; set; }
    }
}