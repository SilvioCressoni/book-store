using System;
using System.Linq;
using Users.Application.Contracts.Response;

namespace Users.Application.Mapper
{
    public class UserMapper : IMapper<Domain.Common.User, User>
    {
        private readonly IMapper<Domain.Common.Phone, Phone> _phoneMapper;
        private readonly IMapper<Domain.Common.Address, Address> _addressMapper;

        public UserMapper(IMapper<Domain.Common.Phone, Phone> phoneMapper, IMapper<Domain.Common.Address, Address> addressMapper)
        {
            _phoneMapper = phoneMapper ?? throw new ArgumentNullException(nameof(phoneMapper));
            _addressMapper = addressMapper ?? throw new ArgumentNullException(nameof(addressMapper));
        }

        public User Map(Domain.Common.User source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            return new User
            {
                Id = source.Id,
                Email = source.Email,
                FirstName = source.FirstName,
                LastNames = source.LastNames,
                BirthDay = source.BirthDay,
                Phones = source.Phones?.Select(x => _phoneMapper.Map(x)),
                Addresses = source.Addresses?.Select(x => _addressMapper.Map(x))
            };
        }
    }
}