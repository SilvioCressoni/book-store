using System;

namespace Users.Application.Contracts.Request
{
    public class UserUpdate
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
