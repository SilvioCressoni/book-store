using System;

namespace Gateway.API.Admin.Web.Contracts.Request
{
    public class AddUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
