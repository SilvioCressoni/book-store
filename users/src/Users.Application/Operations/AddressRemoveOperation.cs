using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts;
using Users.Domain;

namespace Users.Application.Operations
{
    public class AddressRemoveOperation : IOperation<Address>
    {
        private readonly IUserAggregateStore _store;

        public AddressRemoveOperation(IUserAggregateStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task<Result> ExecuteAsync(Address operation, CancellationToken cancellation = default)
        {
            var root = await _store.GetAsync(operation.UserId, cancellation);

            if (root == null)
            {
                return DomainError.UserError.UserNotFound;
            }

            if (root.RemoveAddress(operation.Id) is ErrorResult error)
            {
                return error;
            }

            await _store.SaveAsync(root, cancellation);
            return Result.Ok();
        }
    }
}
