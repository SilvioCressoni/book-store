using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Publisher.Domain.Common;

namespace Publisher.Infrastructure.Repositories
{
    public class PublisherRepositoryInMemory : IPublisherRepository
    {
        private readonly ICollection<Publish> _publishers = new List<Publish>();

        public Task<Publish> GetByIdAsync(Guid id)
        {
            var result = _publishers.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(result);
        }

        public Task SaveAsync(Publish publish)
        {
            if (publish.Id == Guid.Empty)
            {
                publish.Id = Guid.NewGuid();
                _publishers.Add(publish);
            }
            return Task.CompletedTask;
        }
    }
}
