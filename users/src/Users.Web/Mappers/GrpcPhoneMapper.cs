using Users.Application.Mapper;
using PhoneApplication = Users.Application.Contracts.Response.Phone;

namespace Users.Web.Mappers
{
    public class GrpcPhoneMapper : IMapper<PhoneApplication, Phone>
    {
        public Phone Map(PhoneApplication source)
        {
            return new Phone
            {
                Number = source.Number
            };
        }
    }
}
