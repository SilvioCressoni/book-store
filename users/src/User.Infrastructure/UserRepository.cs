using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace User.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ISession _session;

        public UserRepository(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task<Domain.Common.User> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        {
            return _session.GetAsync<Domain.Common.User>(id, cancellation);
        }

        public Task<bool> EmailExistAsync(string email, CancellationToken cancellation = default) 
            => _session.Query<Domain.Common.User>()
                .AnyAsync(x => x.Email == email, cancellation);

        public async Task SaveAsync(Domain.Common.User user, CancellationToken cancellation = default)
        {
            await _session.SaveOrUpdateAsync(user, cancellation);
            await _session.FlushAsync(cancellation);
        }
    }
}