using System;
using System.ComponentModel.DataAnnotations;

namespace Gateway.API.Admin.Web.Contracts.Request
{
    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastNames { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
