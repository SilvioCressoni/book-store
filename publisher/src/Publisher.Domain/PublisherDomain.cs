using System;
using System.Threading.Tasks;
using Publisher.Domain.Common;
using Publisher.Infrastructure.Repositories;

namespace Publisher.Domain
{
    public class PublisherDomain
    {
        private readonly IPublisherRepository _repository;
        private Publish _publish; 

        public PublisherDomain(IPublisherRepository repository)
        {
            _repository = repository;
        }

        public Task SaveAsync()
        {
            return _repository.SaveAsync(_publish);
        }

        public Result Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return DomainError.PublisherError.NameIsEmpty;
            }

            if (name.Length > 250)
            {
                return DomainError.PublisherError.InvalidName;
            }

            _publish = new Publish
            {
                Name = name
            };

            return Result.Ok(_publish);
        }
    }
}
