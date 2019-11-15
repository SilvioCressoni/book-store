using System;
using System.Threading;
using System.Threading.Tasks;
using User.Application.Contracts;
using User.Domain;

namespace User.Application
{
    public class PhoneAddOperation
    {
        private readonly IUserAggregateStore _userAggregateStore;

        public PhoneAddOperation(IUserAggregateStore userAggregateStore)
        {
            _userAggregateStore = userAggregateStore ?? throw new ArgumentNullException(nameof(userAggregateStore));
        }

        public async Task<Result> ExecuteAsync(Phone phone, CancellationToken cancellation = default)
        {
            var root = await _userAggregateStore.GetAsync(phone.UserId, cancellation);

            if (root == null)
            {
                return DomainError.UserError.UserNotFound;
            }

            if (root.AddPhone(phone.Number) is ErrorResult error)
            {
                return error;
            }

            await _userAggregateStore.SaveAsync(root, cancellation);
            return Result.Ok();
        }
    }
}
