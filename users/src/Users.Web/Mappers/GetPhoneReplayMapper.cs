using System;
using System.Collections.Generic;
using Users.Application.Mapper;
using Users.Domain;
using PhoneApplication = Users.Application.Contracts.Response.Phone;

namespace Users.Web
{
    public class GetPhoneReplayMapper : IMapper<Result, GetPhoneReplay>
    {
        private readonly IMapper<PhoneApplication, Phone> _phoneMapper;

        public GetPhoneReplayMapper(IMapper<PhoneApplication, Phone> phoneMapper)
        {
            _phoneMapper = phoneMapper ?? throw new ArgumentNullException(nameof(phoneMapper));
        }

        public GetPhoneReplay Map(Result source)
        {
            var replay = new GetPhoneReplay
            {
                IsSuccess = false,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };

            if (source is OkResult<IEnumerable<PhoneApplication>> okResult)
            {
                foreach (var phone in okResult.Value)
                {
                    replay.Value.Add(_phoneMapper.Map(phone));
                }
            }

            return replay;
        }
    }
}
