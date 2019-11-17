using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Domain;

namespace Users.Application.Operations
{
    public class AddressAddOperation : IOperation<AddressAdd>
    {
        private readonly IUserAggregateStore _store;

        public AddressAddOperation(IUserAggregateStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async ValueTask<Result> ExecuteAsync(AddressAdd operation, CancellationToken cancellation = default)
        {
            var root = await _store.GetAsync(operation.UserId, cancellation);

            if (root == null)
            {
                return DomainError.UserError.UserNotFound;
            }

            if (root.AddAddress(operation.Line, operation.Number, operation.PostCode) is ErrorResult error)
            {
                return error;
            }

            await _store.SaveAsync(root, cancellation);
            var address = root.State.Addresses.Last();
            return Result.Ok(new Address
            {
                Id = address.Id,
                Line = operation.Line,
                Number = operation.Number,
                PostCode = operation.PostCode
            });
        }
    }
}
