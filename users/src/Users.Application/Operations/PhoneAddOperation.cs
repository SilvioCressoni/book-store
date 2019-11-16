using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts;
using Users.Domain;

namespace Users.Application.Operations
{
    public class PhoneAddOperation : IOperation<Phone>
    {
        private readonly IUserAggregateStore _store;

        public PhoneAddOperation(IUserAggregateStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task<Result> ExecuteAsync(Phone operation, CancellationToken cancellation = default)
        {
            var root = await _store.GetAsync(operation.UserId, cancellation);

            if (root == null)
            {
                return DomainError.UserError.UserNotFound;
            }

            if (root.AddPhone(operation.Number) is ErrorResult error)
            {
                return error;
            }

            await _store.SaveAsync(root, cancellation);
            return Result.Ok(operation);
        }
    }
}
