using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Web.Proto;

namespace Gateway.Web.Controllers
{
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly Users.Web.Proto.Users.UsersClient _client;

        public UserController(Users.Web.Proto.Users.UsersClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<User> GetUsersAsync([FromRoute]int? skip, [FromRoute]int? take, CancellationToken cancellation = default)
        {
            var stream = _client.GetUsers(new GetUsersRequest {Skip = skip, Take = take});
            var reader = stream.ResponseStream;
            while (await reader.MoveNext(cancellation))
            {
                yield return reader.Current.Value;
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserAsync([FromRoute] string id, CancellationToken cancellation = default)
        {
            var result = await _client.GetUserAsync(new GetUserRequest {UserId = id}, cancellationToken: cancellation);
            if (!result.IsSuccess)
            {
                return result.ErrorCode == "USR005" ? NotFound(result) : StatusCode(StatusCodes.Status422UnprocessableEntity, result);
            }

            return Ok(result);
        } 
    }
}
