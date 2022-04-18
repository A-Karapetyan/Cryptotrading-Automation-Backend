using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DAL.Entity
{
    public class Symptom : BaseEntity
    {
        public ICollection<Criteria> Criterias { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
