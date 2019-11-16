using System;
using System.Collections.Generic;

namespace Users.Domain.Common
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastNames { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime BirthDay { get; set; }
        
        public virtual ISet<Phone> Phones { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
