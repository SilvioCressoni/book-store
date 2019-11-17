using System;
using System.Collections.Generic;

namespace Users.Application.Contracts.Response
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public DateTime BirthDay { get; set; }

        public IEnumerable<Phone> Phones { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}
