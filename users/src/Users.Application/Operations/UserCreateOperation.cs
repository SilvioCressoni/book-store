using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Domain;
using Users.Infrastructure;

namespace Users.Application.Operations
{
    public class UserCreateOperation : IOperation<UserAdd>
    {
        private readonly IUserAggregateStore _store;
        private readonly IReadOnlyUserRepository _repository;
        private readonly ILogger<UserCreateOperation> _logger;

        public UserCreateOperation(IUserAggregateStore store, IReadOnlyUserRepository repository, ILogger<UserCreateOperation> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<Result> ExecuteAsync(UserAdd operation, CancellationToken cancellation = default)
        {
            var scope = _logger.BeginScope("Creating user.");
            try
            {
                var root = _store.Create();

                if (root.Create(operation.Email, operation.FirstName,
                    operation.LastNames, operation.BirthDay) is ErrorResult error)
                {
                    _logger.LogInformation("Error to create user: {0}", error.ErrorCode);
                    return error;
                }

                if (await _repository.EmailExistAsync(operation.Email, cancellation))
                {
                    _logger.LogInformation("Email already exist");
                    return DomainError.UserError.EmailAlreadyExist;
                }

                await _store.SaveAsync(root, cancellation);
                _logger.LogInformation("User created: [UserId: {0}]", root.State.Id );
                return Result.Ok(new User
                {
                    Id = root.State.Id,
                    Email = operation.Email,
                    FirstName = operation.FirstName,
                    LastNames = operation.LastNames,
                    BirthDay = operation.BirthDay
                });
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error to create user");
                return Result.Fail(e.HResult.ToString(), e.ToString());
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
}
