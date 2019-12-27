using Users.Application.Mapper;
using Users.Web.Proto;
using PhoneResponse = Users.Application.Contracts.Response.Phone;

namespace Users.Web.Mappers
{
    public class GrpcPhoneMapper : IMapper<PhoneResponse, Phone>
    {
        public Phone Map(PhoneResponse source)
        {
            return new Phone
            {
                Number = source.Number
            };
        }
    }
}
