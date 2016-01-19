using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.Message
{
    public class Message : PersistentEntity<int>
    {
        [Required]
        [EmailAddress]
        [MaxLength(50, ErrorMessage = "Sender Email should be less than 50 cahracters.")]
        [DisplayName("Email")]
        public string SenderEmail { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "About should be less than 100 characters.")]
        public string About { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Text should be less than 500 characters.")]
        public string Text { get; set; }
    }
}
