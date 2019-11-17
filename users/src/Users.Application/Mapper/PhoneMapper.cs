using System;
using Users.Application.Contracts.Response;

namespace Users.Application.Mapper
{
    public class PhoneMapper : IMapper<Domain.Common.Phone, Phone>
    {
        public Phone Map(Domain.Common.Phone source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            return new Phone
            {
                Number = source.Number
            };
        }
    }
}