using DatabaseEntities.Others;
using System.Collections.Generic;

namespace PersonalWebsite.Models
{
    public class BlogViewModel
    {
        public int? AuthorId { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}