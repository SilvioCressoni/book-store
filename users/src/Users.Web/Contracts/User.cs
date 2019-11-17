using System;
using System.Collections.Generic;

namespace Users.Web.Contracts
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public DateTime BirthDay { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
