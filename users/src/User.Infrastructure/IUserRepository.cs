using System.Threading.Tasks;

namespace User.Infrastructure
{
    public interface IUserRepository : IReadOnlyUserRepository
    {
        Task SaveAsync(Domain.Common.User user);
    }
}
