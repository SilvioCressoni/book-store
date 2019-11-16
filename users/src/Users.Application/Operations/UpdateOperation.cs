using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts;
using Users.Domain;

namespace Users.Application.Operations
{
    public class UpdateOperation : IOperation<User>
    {
        private readonly IUserAggregateStore _store;

        public UpdateOperation(IUserAggregateStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task<Result> ExecuteAsync(User operation, CancellationToken cancellation = default)
        {
            var root = await _store.GetAsync(operation.Id, cancellation);

            if (root == null)
            {
                return DomainError.UserError.UserNotFound;
            }

            if (root.Update(operation.FirstName, operation.LastNames, operation.BirthDay) is ErrorResult error)
            {
                return error;
            }

            await _store.SaveAsync(root, cancellation);

            return Result.Ok(new User
            {
                Id = root.State.Id,
                Email = operation.Email,
                FirstName = operation.FirstName,
                LastNames = operation.LastNames,
                BirthDay = operation.BirthDay
            });
        }
    }
}
