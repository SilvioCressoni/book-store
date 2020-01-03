using System;
using System.Threading.Tasks;
using Polly;
using Users.Web.Proto;

namespace Gateway.Service
{
    public class UserService : IUserService
    {
        private readonly IAsyncPolicy _policy;
        private readonly Users.Web.Proto.Users.UsersClient _client;

        public UserService(Users.Web.Proto.Users.UsersClient client, IAsyncPolicy policy)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task<StatusCodeResult> GetAsync(string id)
        {
            var result = await _policy.ExecuteAsync(async () => await _client.GetUserAsync(new GetUserRequest
            {
                UserId = id
            }));

            throw new NotImplementedException();
        }
    }
}
