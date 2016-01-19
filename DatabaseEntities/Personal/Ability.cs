using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseEntities.Personal
{
    public class Ability : PersistentEntity<int>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Ability)
            {
                Ability other = (Ability)obj;
                return Id == other.Id &&
                    string.Equals(Name, other.Name) &&
                    string.Equals(Description, other.Description) &&
                    PersonId == other.PersonId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (((Name.GetHashCode() * 31 * Description.GetHashCode()) ^ Id) * 37) ^ PersonId;
        }
    }
}
