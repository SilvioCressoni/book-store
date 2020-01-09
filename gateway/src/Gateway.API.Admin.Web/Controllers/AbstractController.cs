using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Admin.Web.Controllers
{
    public abstract class AbstractController : Controller
    {
        [NonAction]
        public IActionResult StatusCode(HttpStatusCode statusCode, object value)
        {
            if (statusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }

            return StatusCode((int)statusCode, value);
        }
    }
}
