using System;

namespace Gateway.API.Admin.Web.Contracts.Request
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
