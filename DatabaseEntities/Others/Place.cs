using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.Others
{
    public class Place : PersistentEntity<int>
    {
        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude should be between -180 and 180.")]
        public double Longitude { get; set; }
        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude should be between -90 and 90.")]
        public double Latitude { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayName("Photo")]
        public string PhotoURL { get; set; }
    }
}
