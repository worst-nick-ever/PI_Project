using System.Collections.Generic;
using DatabaseEntities.Personal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.CV
{
    public class CV : PersistentEntity<int>
    {
        [Required]
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public virtual ICollection<WorkEntry> WorkEntries { get; set; }
    }
}
