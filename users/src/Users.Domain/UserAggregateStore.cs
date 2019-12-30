using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Users.Domain.Common;
using Users.Domain.Event;
using Users.Domain.Interceptors;
using Users.Infrastructure;

namespace Users.Domain
{
    public class UserAggregateStore : IUserAggregateStore, IAggregationRootInterceptor<UserState, Guid>
    {
        private readonly IUserRepository _repository;
        private readonly ICollection<Phone> _phones;
        private readonly ICollection<Address> _addresses;
        private readonly ILoggerFactory _loggerFactory;

        public UserAggregateStore(IUserRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _phones = new LinkedList<Phone>();
            _addresses = new LinkedList<Address>();
        }

        public IUserAggregationRoot Create()
            => new UserAggregationRoot(new UserState(new Common.User()), new[] { this }, 
                _loggerFactory.CreateLogger<UserAggregationRoot>());

        public async Task<IUserAggregationRoot> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var user = await _repository.GetByIdAsync(id, cancellation);
            return user == null ? null : new UserAggregationRoot(new UserState(user), new[] { this }, 
                _loggerFactory.CreateLogger<UserAggregationRoot>());
        }

        public async Task SaveAsync(IUserAggregationRoot aggregate, CancellationToken cancellation = default)
        {
            foreach (var phone in _phones)
            {
                await _repository.RemoveAsync(phone, cancellation);
            }

            foreach (var address in _addresses)
            {
                await _repository.RemoveAsync(address, cancellation);
            }

            await  _repository.SaveAsync((Common.User) aggregate.State, cancellation)
               .ConfigureAwait(false);
        }

        public void OnApply(UserState state, IEvent @event)
        {
            switch (@event)
            {
                case PhoneRemoveEvent phoneRemoveEvent:
                    _phones.Add(state.Phones.First(x => x.Number == phoneRemoveEvent.Number));
                    break;

                case AddressRemoveEvent addressRemoveEvent:
                    _addresses.Add(state.Addresses.First(x => x.Id == addressRemoveEvent.Id));
                    break;
            }
        }
    }
}
