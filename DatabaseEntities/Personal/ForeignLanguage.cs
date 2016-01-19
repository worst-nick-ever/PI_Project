using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseEntities.Personal
{
    public class ForeignLanguage : PersistentEntity<int>
    {
        [Required, MaxLength(30, ErrorMessage = "Name must be less than 30 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Language name can contain only small and uppercase letters.")]
        public string Name { get; set; }
        public virtual ICollection<Person> People { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is ForeignLanguage)
            {
                ForeignLanguage other = (ForeignLanguage)obj;
                return Id == other.Id && string.Equals(Name, other.Name);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Name.GetHashCode() * 31) ^ Id;
        }
    }
}
