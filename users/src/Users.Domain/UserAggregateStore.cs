using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Users.Domain.Common;
using Users.Domain.Event;
using Users.Infrastructure;

namespace Users.Domain
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

        public async Task SaveAsync(IUserAggregationRoot aggregate, CancellationToken cancellation = default)
        {
            foreach (var @event in aggregate.Events)
            {
                switch (@event)
                {
                    case PhoneRemoveEvent phoneRemoveEvent:
                        var phone = aggregate.State.Phones.First(x => x.Number == phoneRemoveEvent.Number);
                        aggregate.Apply(phoneRemoveEvent);
                        phone.User = null;
                        await _repository.RemoveAsync(phone, cancellation)
                            .ConfigureAwait(false);
                        break;
                    case AddressRemoveEvent address:
                        break;
                    default:
                        aggregate.Apply(@event);
                        break;
                }
            }
            
           await  _repository.SaveAsync((Common.User) aggregate.State, cancellation)
               .ConfigureAwait(false);
        }
    }
}
