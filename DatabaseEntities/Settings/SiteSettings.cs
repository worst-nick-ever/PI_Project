using DatabaseEntities.Personal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseEntities.Settings
{
    public class SiteSettings : PersistentEntity<int>
    {
        [DisplayName("CV")]
        public int? CVId { get; set; }
        [ForeignKey("CVId")]
        public CV.CV CV { get; set; }
        [DisplayName("Person")]
        public int? PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        [DisplayName("Theme")]
        public int? ThemeId { get; set; }
        [ForeignKey("ThemeId")]
        public Theme Theme { get; set; }
    }
}
