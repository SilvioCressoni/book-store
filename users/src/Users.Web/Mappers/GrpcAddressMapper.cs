using Users.Application.Mapper;
using Users.Web.Proto;
using AddressResponse = Users.Application.Contracts.Response.Address;

namespace Users.Web.Mappers
{
    public class GrpcAddressMapper : IMapper<AddressResponse, Address>
    {
        public Address Map(AddressResponse source)
        {
            return new Address
            {
                Id = source.Id.ToString(),
                Number = source.Number,
                Line = source.Line,
                PostCode = source.PostCode
            };
        }
    }
}
