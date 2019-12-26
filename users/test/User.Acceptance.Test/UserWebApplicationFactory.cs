using Microsoft.AspNetCore.Mvc.Testing;

namespace User.Acceptance.Test
{
    public class UserWebApplicationFactory : WebApplicationFactory<Users.Web.Startup>
    {
    }
}