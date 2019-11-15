using System;
using System.Threading.Tasks;
using NHibernate;

namespace User.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ISession _session;

        public UserRepository(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task<Domain.Common.User> GetById(Guid id)
        {
            return _session.LoadAsync<Domain.Common.User>(id);
        }

        public async Task SaveAsync(Domain.Common.User user)
        {
            await _session.SaveOrUpdateAsync(user);
            await _session.FlushAsync();
        }
    }
}