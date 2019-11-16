using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using Users.Domain.Common;

namespace Users.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ISession _session;

        public UserRepository(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task<User> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        {
            return _session.GetAsync<User>(id, cancellation);
        }

        public Task<bool> EmailExistAsync(string email, CancellationToken cancellation = default) 
            => _session.Query<User>()
                .AnyAsync(x => x.Email == email, cancellation);

        public async Task SaveAsync(User user, CancellationToken cancellation = default)
        {
            await _session.SaveOrUpdateAsync(user, cancellation);
            await _session.FlushAsync(cancellation);
        }
    }
}