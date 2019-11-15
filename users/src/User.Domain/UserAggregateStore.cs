using System;
using System.Threading;
using System.Threading.Tasks;
using User.Infrastructure;

namespace User.Domain
{
    public class UserAggregateStore : IUserAggregateStore
    {
        private readonly IUserRepository _repository;
        public UserAggregateStore(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IUserAggregationRoot Create() 
            => new UserAggregationRoot(new UserState(new Common.User()));

        public async Task<IUserAggregationRoot> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var user = await _repository.GetByIdAsync(id, cancellation);
            return user == null ? null : new UserAggregationRoot(new UserState(user));
        }

        public Task SaveAsync(IUserAggregationRoot aggregate, CancellationToken cancellation = default) 
            => _repository.SaveAsync((Common.User) aggregate.State, cancellation);
    }
}
