using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Users.Domain.Common;

namespace Users.Infrastructure
{
    public interface IReadOnlyUserRepository
    {
        Task<User> GetByIdAsync(Guid id, CancellationToken cancellation = default);
        Task<bool> EmailExistAsync(string email, CancellationToken cancellation = default);
        IEnumerable<User> GetAll(int skip, int take);
    }
}