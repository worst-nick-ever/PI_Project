using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntities
{
    public class PersistentEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
