using System;
using System.Threading.Tasks;
using Publisher.Application.Contracts.Request;
using Publisher.Domain;

namespace Publisher.Application.Operations
{
    public class PublisherAddOperation : IOperation<PublisherAdd>
    {
        private readonly PublisherDomain _domain;

        public PublisherAddOperation(PublisherDomain domain)
        {
            _domain = domain;
        }

        public async Task<Result> ExecuteAsync(PublisherAdd request)
        {
            var result = _domain.Create(request.Name);

            if(result.IsSuccess)
            {
               await _domain.SaveAsync();
            }
            return result;
        }
    }
}
