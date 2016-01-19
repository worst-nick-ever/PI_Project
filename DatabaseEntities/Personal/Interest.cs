using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.Personal
{
    public class Interest : PersistentEntity<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desctiption { get; set; }
        public string Photo { get; set; }
        [RegularExpression("^((http|https)://)?www.youtube.com/embed/[a-zA-Z0-9-_]{10,}$", ErrorMessage = "The given text is not a valid youtube embed URL.")]
        public string VideoURL { get; set; }
    }
}
