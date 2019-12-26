using System;
using Users.Application.Mapper;
using Users.Domain;
using PhoneApplication = Users.Application.Contracts.Response.Phone;

namespace Users.Web
{
    public class AddPhoneReplayMapper : IMapper<Result, AddPhoneReplay>
    {
        private readonly IMapper<PhoneApplication, Phone> _phoneMapper;

        public AddPhoneReplayMapper(IMapper<PhoneApplication, Phone> phoneMapper)
        {
            _phoneMapper = phoneMapper;
        }

        public AddPhoneReplay Map(Result source)
        {
            switch (source)
            {
                case ErrorResult error:
                    return new AddPhoneReplay
                    {
                        IsSuccess = false,
                        Description = error.Description,
                        ErrorCode = error.ErrorCode
                    };
                case OkResult<PhoneApplication> okResult:
                {
                    return new AddPhoneReplay
                    {
                        IsSuccess = true, 
                        Value = _phoneMapper.Map(okResult.Value)
                    };
                }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
