using System;
using System.Collections.Generic;
using Users.Application.Mapper;
using Users.Domain;
using PhoneApplication = Users.Application.Contracts.Response.Phone;

namespace Users.Web.Mappers
{
    public class GetPhoneReplayMapper : IMapper<Result, GetPhoneReplay>
    {
        private readonly IMapper<PhoneApplication, Phone> _mapper;

        public GetPhoneReplayMapper(IMapper<PhoneApplication, Phone> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public GetPhoneReplay Map(Result source)
        {
            var replay = new GetPhoneReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };

            if (source is OkResult<IEnumerable<PhoneApplication>> okResult)
            {
                foreach (var phone in okResult.Value)
                {
                    replay.Value.Add(_mapper.Map(phone));
                }
            }

            return replay;
        }
    }
}
