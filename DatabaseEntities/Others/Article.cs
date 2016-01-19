using System.Collections.Generic;
using DatabaseEntities.Personal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.Others
{
    public class Article : PersistentEntity<int>
    {
        [Required]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Person Author { get; set; }
        [Required,
         MaxLength(200, ErrorMessage = "Title must be less than 200 characters")]
        public string Title { get; set; }
        public virtual ICollection<Paragraph> Paragraphs { get; set; }
    }
}
