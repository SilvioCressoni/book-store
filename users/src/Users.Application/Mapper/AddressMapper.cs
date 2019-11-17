using System;
using Users.Application.Contracts.Response;

namespace Users.Application.Mapper
{
    public class AddressMapper : IMapper<Domain.Common.Address, Address>
    {
        public Address Map(Domain.Common.Address source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            return new Address
            {
                Id = source.Id,
                Line = source.Line,
                PostCode = source.PostCode,
                Number = source.Number
            };
        }
    }
}