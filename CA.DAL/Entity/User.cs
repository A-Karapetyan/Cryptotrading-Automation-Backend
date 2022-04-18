using System.Collections.Generic;

namespace CA.DAL.Entity
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public ICollection<Symptom>  Symptoms { get; set; }

    }
}
