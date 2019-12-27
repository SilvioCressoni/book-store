using System;
using Google.Protobuf.WellKnownTypes;
using Users.Application.Mapper;
using Users.Web.Proto;
using UserResponse = Users.Application.Contracts.Response.User;
using PhoneResponse = Users.Application.Contracts.Response.Phone;
using AddressResponse = Users.Application.Contracts.Response.Address;

namespace Users.Web.Mappers
{
    public class GrpcUserMapper : IMapper<UserResponse, User>
    {
        private readonly IMapper<PhoneResponse, Phone> _phoneMapper;
        private readonly IMapper<AddressResponse, Address> _addressMapper;

        public GrpcUserMapper(IMapper<PhoneResponse, Phone> phoneMapper, IMapper<AddressResponse, Address> addressMapper)
        {
            _phoneMapper = phoneMapper ?? throw new ArgumentNullException(nameof(phoneMapper));
            _addressMapper = addressMapper ?? throw new ArgumentNullException(nameof(addressMapper));
        }

        public User Map(UserResponse source)
        {
            var user = new User
            {
                Id = source.Id.ToString(),
                FirstName = source.FirstName,
                LastNames = source.LastNames,
                BirthDate = source.BirthDay.ToTimestamp(),
                Email = source.Email
            };

            if (source.Phones != null)
            {
                foreach (var phone in source.Phones)
                {
                    user.Phones.Add(_phoneMapper.Map(phone));
                }
            }

            if (source.Addresses != null)
            {
                foreach (var address in source.Addresses)
                {
                    user.Addresses.Add(_addressMapper.Map(address));
                }
            }

            return user;
        }
    }
}
