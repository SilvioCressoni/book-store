using System.ComponentModel.DataAnnotations;

namespace Gateway.API.Admin.Web.Contracts.Request
{
    public class AddPhoneRequest
    {
        [Required]
        public string Number { get; set; }
    }
}
