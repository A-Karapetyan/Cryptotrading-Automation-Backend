using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DAL.Entity
{
    public class TemporaryUser: BaseEntity
    {
        public string Email { get; set; }
        public string EmailCode { get; set; }
        public bool EmailVerified { get; set; }
    }
}
