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
            _phoneMapper = phoneMapper;
        }

        public GetPhoneReplay Map(Result source)
        {
            switch (source)
            {
                case ErrorResult error:
                    return new GetPhoneReplay
                    {
                        IsSuccess = false,
                        Description = error.Description,
                        ErrorCode = error.ErrorCode
                    };
                case OkResult<IEnumerable<PhoneApplication>> okResult:
                {
                    var replay = new GetPhoneReplay
                    {
                        IsSuccess = true
                    };

                    foreach (var phone in okResult.Value)
                    {
                        replay.Value.Add(_phoneMapper.Map(phone));
                    }

                    return replay;
                }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
