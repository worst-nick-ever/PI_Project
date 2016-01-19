using System;
using System.Collections.Generic;
using DatabaseEntities.Others;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DatabaseEntities.Personal
{
    public class Person : PersistentEntity<int>
    {
        [DisplayName("First Name")]
        [Required,
         MaxLength(50, ErrorMessage = "First Name must be less than 50 characters")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        [Required,
         MaxLength(50, ErrorMessage = "Middle Name must be less than 50 characters")]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        [Required,
         MaxLength(50, ErrorMessage = "Last Name must be less than 50 characters")]
        public string LastName { get; set; }
        [DisplayName("Date of birth")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public int ContactsId { get; set; }
        [ForeignKey("ContactsId")]
        public Contacts Contacts { get; set; }
        [DisplayName("Photo")]
        public string PhotoFileName { get; set; }
        public virtual ICollection<Ability> OtherAbilities { get; set; }
        public virtual ICollection<ForeignLanguage> ForeignLanguages { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<Place> Places { get; set; }
    }
}
