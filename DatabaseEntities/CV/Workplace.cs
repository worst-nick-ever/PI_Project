using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.CV
{
    public class Workplace : PersistentEntity<int>
    {
        [Required,
         MaxLength(50, ErrorMessage = "Name must be less than 50 characters")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
