using System.Threading;
using System.Threading.Tasks;
using Users.Domain.Common;

namespace Users.Infrastructure
{
    public interface IUserRepository : IReadOnlyUserRepository
    {
        Task RemoveAsync(Address address, CancellationToken cancellation = default);
        Task RemoveAsync(Phone phone, CancellationToken cancellation = default);
        Task SaveAsync(User user, CancellationToken cancellation = default);
    }
}
