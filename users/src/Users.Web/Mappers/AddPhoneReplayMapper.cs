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
            _phoneMapper = phoneMapper ?? throw new ArgumentNullException(nameof(phoneMapper));
        }

        public AddPhoneReplay Map(Result source)
        {
            var replay = new AddPhoneReplay
            {
                IsSuccess = false,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };

            if (source is OkResult<PhoneApplication> okResult)
            {
                replay.Value = _phoneMapper.Map(okResult.Value);
            }

            return replay;
        }
    }
}
