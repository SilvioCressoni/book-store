using System;

namespace Users.Application.Contracts.Request
{
    public class UserAdd
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
