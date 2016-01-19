using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.Personal
{
    public class Contacts : PersistentEntity<int>
    {
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
    }
}
