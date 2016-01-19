using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.Settings
{
    public class Theme : PersistentEntity<int>
    {
        [Required, MaxLength(20, ErrorMessage = "Name must be less than 20 cahracters.")]
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
    }
}
