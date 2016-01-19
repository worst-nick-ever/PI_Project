using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseEntities.Others
{
    public class Paragraph : PersistentEntity<int>
    {
        [Required]
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
        [MaxLength(200, ErrorMessage = "Title must be less than 100 characters")]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
