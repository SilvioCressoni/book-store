using Microsoft.AspNetCore.Mvc;

namespace Users.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/swagger"); 
        }
    }
}