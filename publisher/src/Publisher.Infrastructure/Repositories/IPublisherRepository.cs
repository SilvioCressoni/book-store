using System;
using System.Threading.Tasks;
using Publisher.Domain.Common;

namespace Publisher.Infrastructure.Repositories
{
    public interface IPublisherRepository
    {

        Task<Publish> GetByIdAsync(Guid id);
        Task SaveAsync(Publish publish);
    }
}
