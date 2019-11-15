using System;
using System.Threading;
using System.Threading.Tasks;

namespace User.Infrastructure
{
    public interface IReadOnlyUserRepository
    {
        Task<Domain.Common.User> GetByIdAsync(Guid id, CancellationToken cancellation = default);
        Task<bool> EmailExistAsync(string email, CancellationToken cancellation = default);
    }
}