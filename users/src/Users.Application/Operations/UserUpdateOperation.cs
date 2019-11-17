using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Domain;

namespace Users.Application.Operations
{
    public class UserUpdateOperation : IOperation<UserUpdate>
    {
        private readonly IUserAggregateStore _store;
        private readonly ILogger<UserUpdateOperation> _logger;

        public UserUpdateOperation(IUserAggregateStore store, ILogger<UserUpdateOperation> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<Result> ExecuteAsync(UserUpdate operation, CancellationToken cancellation = default)
        {
            var scope = _logger.BeginScope("Updating user. [UserId: {0}]", operation.Id);
            try
            {
                var root = await _store.GetAsync(operation.Id, cancellation);

                if (root == null)
                {
                    _logger.LogInformation("User not found");
                    return DomainError.UserError.UserNotFound;
                }

                if (root.Update(operation.FirstName, operation.LastNames, operation.BirthDay) is ErrorResult error)
                {
                    _logger.LogInformation("Error to update user. [ErrorCode: {0}]", error.ErrorCode);
                    return error;
                }

                await _store.SaveAsync(root, cancellation);
                _logger.LogInformation("User updated.");
                return Result.Ok(new User
                {
                    Id = root.State.Id,
                    Email = root.State.Email,
                    FirstName = operation.FirstName,
                    LastNames = operation.LastNames,
                    BirthDay = operation.BirthDay
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error to update user. [UserId: {0}]", operation.Id);
                return Result.Fail(e.HResult.ToString(), e.ToString());
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
}
