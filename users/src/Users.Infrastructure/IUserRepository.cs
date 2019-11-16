using System.Threading;
using System.Threading.Tasks;
using Users.Domain.Common;

namespace Users.Infrastructure
{
    public interface IUserRepository : IReadOnlyUserRepository
    {
        Task SaveAsync(User user, CancellationToken cancellation = default);
    }
}
