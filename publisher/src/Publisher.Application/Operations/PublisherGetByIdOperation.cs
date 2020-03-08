using System;
using System.Threading.Tasks;
using Publisher.Application.Contracts.Request;
using Publisher.Domain;
using Publisher.Infrastructure.Repositories;

namespace Publisher.Application.Operations
{
    public class PublisherGetByIdOperation : IOperation<PublisherGetById>
    {
        private readonly IPublisherRepository _repository;

        public PublisherGetByIdOperation(IPublisherRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> ExecuteAsync(PublisherGetById request)
        {
            var result = await _repository.GetByIdAsync(request.Id);
            if (result != null)
            {
                return Result.Ok(result);
            }

            return DomainError.PublisherError.NotFound;
        }
    }
}
