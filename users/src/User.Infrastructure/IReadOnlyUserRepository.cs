using System;
using System.Threading.Tasks;

namespace User.Infrastructure
{
    public interface IReadOnlyUserRepository
    {
        Task<Domain.Common.User> GetById(Guid id);
    }
}