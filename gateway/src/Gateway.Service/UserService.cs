using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Polly;
using Users.Web.Proto;

using static Gateway.Service.IStatusCodeResult;

namespace Gateway.Service
{
    public class UserService : IUserService
    {
        private readonly IAsyncPolicy _policy;
        private readonly Users.Web.Proto.Users.UsersClient _client;
        private readonly ILogger<UserService> _logger;
        
        private const string NOT_FOUND = "USR005";
        private const string INVALID_ID = "USR007";

        public UserService(Users.Web.Proto.Users.UsersClient client, 
            IAsyncPolicy policy, 
            ILogger<UserService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region User
        public async Task<IStatusCodeResult> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting User. {0}", id);
            var result = await _policy.ExecuteAsync(async () => await _client.GetUserAsync(new GetUserRequest
                {
                    UserId = id
                }, 
                    headers: new Metadata(),
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);

            _logger.LogInformation(result.ToString());
            return Map(result);
        }
        
        public async Task<IStatusCodeResult> AddUserAsync(string email, string firstName, string lastName, 
            DateTime birthDate, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Add User. {0}");
            var result = await _policy.ExecuteAsync(async () => await _client.AddUsersAsync(new AddUserRequest
                {
                    Email = email,
                    FirstName = firstName,
                    LastNames = lastName,
                    BirthDate = birthDate.ToTimestamp()
                },
                    headers: new Metadata(),
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);

            _logger.LogInformation(result.ToString());
            return Map(result, HttpStatusCode.Created);
        }
        
        public async Task<IStatusCodeResult> UpdateUserAsync(string id, string firstName, 
            string lastName, DateTime birthDate, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Add User. {0}");
            var result = await _policy.ExecuteAsync(async () => await _client.UpdateUserAsync(new UpdateUserRequest
                {
                    Id = id,
                    FirstName = firstName,
                    LastNames = lastName,
                    BirthDate = birthDate.ToTimestamp()
                },
                    headers: new Metadata(), 
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);

            _logger.LogInformation(result.ToString());
            return Map(result);
        }

        #endregion

        #region Phone
        public async Task<IStatusCodeResult> GetPhonesAsync(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting Phone.");
            var result = await _policy.ExecuteAsync(async () => await _client.GetPhonesAsync(new GetPhoneRequest
            {
                UserId = userId
            }, 
                    headers: new Metadata(),
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            
            return Map(result);
        }
        
        public async Task<IStatusCodeResult> AddPhoneAsync(string userId, string number, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Add phone");
            var result = await _policy.ExecuteAsync(async () => await _client.AddPhoneAsync(new AddPhoneRequest
                {
                    UserId = userId,
                    Number = number
                }, 
                    headers: new Metadata(),
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            
            return Map(result, HttpStatusCode.Created);
        }

        public async Task<IStatusCodeResult> RemovePhoneAsync(string userId, string number, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Add phone");
            var result = await _policy.ExecuteAsync(async () => await _client.RemovePhoneAsync(new RemovePhoneRequest
                {
                    UserId = userId,
                    Number = number
                },
                    headers: new Metadata(),
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            return Map(result);
        }

        #endregion

        #region Address

        public async Task<IStatusCodeResult> GetAddressesAsync(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Get address");
            var result = await _policy.ExecuteAsync(async () => await _client.GetAddressesAsync(new GetAddressesRequest
                {
                    UserId = userId
                }, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            return Map(result);
        }

        public async Task<IStatusCodeResult> AddAddressAsync(string userId, string line, int number, string postCode, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Add address");
            var result = await _policy.ExecuteAsync(async () => await _client.AddAddressAsync(new AddAddressRequest
                {
                    UserId = userId,
                    Line = line,
                    Number = number,
                    PostCode = postCode
                }, 
                    headers: new Metadata(),
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            return Map(result);
        }

        public async Task<IStatusCodeResult> RemoveAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Remove address");
            var result = await _policy.ExecuteAsync(async () => await _client.RemoveAddressAsync(new RemoveAddressRequest
                {
                    UserId = userId,
                    Id = addressId
                }, 
                    headers: new Metadata(),
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            return Map(result);
        }

        #endregion
        
        private IStatusCodeResult Map<T>(IReply<T> reply, HttpStatusCode defaultSuccess = HttpStatusCode.OK)
        {
            _logger.LogInformation(reply.ToStringLog());
            if (reply.IsSuccess)
            {
                return new StatusCodeResult<T>(defaultSuccess, reply.Value);
            }
            return reply.ErrorCode switch
            {
                NOT_FOUND => NotFound(reply.Value),
                INVALID_ID => BadRequest(reply.Value),
                _ => UnprocessableEntity(reply.Value)
            };
        }
    }
}
