using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Domain;

namespace Users.Application.Operations
{
    public class PhoneRemoveOperation : IOperation<PhoneRemove>
    {
        private readonly IUserAggregateStore _store;

        public PhoneRemoveOperation(IUserAggregateStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async ValueTask<Result> ExecuteAsync(PhoneRemove operation, CancellationToken cancellation = default)
        {
            var root = await _store.GetAsync(operation.UserId, cancellation);
            if (root == null)
            {
                return DomainError.UserError.UserNotFound;
            }

            if (root.RemovePhone(operation.Number) is ErrorResult error)
            {
                return error;
            }

            await _store.SaveAsync(root, cancellation);
            return Result.Ok(new Phone
            {
                Number = operation.Number
            });
        }
    }
}
