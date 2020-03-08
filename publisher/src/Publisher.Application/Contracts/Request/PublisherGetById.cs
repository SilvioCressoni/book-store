using System;
namespace Publisher.Application.Contracts.Request
{
    public class PublisherGetById
    {
        public PublisherGetById(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

    }
}
