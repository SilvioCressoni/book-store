using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Domain;
using Users.Infrastructure;

namespace Users.Application.Operations
{
    public class UserGetOperation : IOperation<UserGet>
    {
        private readonly IReadOnlyUserRepository _repository;
        private readonly ILogger<UserGetOperation> _logger;

        public UserGetOperation(IReadOnlyUserRepository repository, ILogger<UserGetOperation> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<Result> ExecuteAsync(UserGet operation, CancellationToken cancellation = default)
        {
            var scope = _logger.BeginScope("Get user. [UserId: {0}]", operation.Id);
            try
            {
                var user = await _repository.GetByIdAsync(operation.Id, cancellation);
                if (user == null)
                {
                    return DomainError.UserError.UserNotFound;
                }

                return Result.Ok(new User
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName, 
                    LastNames = user.LastNames,
                    BirthDay = user.BirthDay,
                    Phones = user.Phones.Select(x=>new Phone
                    {
                        Number = x.Number
                    }),
                    Addresses =  user.Addresses.Select(x => new Address
                    {
                        Id = x.Id,
                        Line = x.Line,
                        Number = x.Number,
                        PostCode = x.PostCode
                    })
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error to get user");
                return Result.Fail(e.HResult.ToString(), e.ToString());
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
}
