using System;
using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Proto;
using PhoneApplication = Users.Application.Contracts.Response.Phone;

namespace Users.Web.Mappers
{
    public class AddPhoneReplayMapper : IMapper<Result, AddPhoneReplay>
    {
        private readonly IMapper<PhoneApplication, Phone> _mapper;

        public AddPhoneReplayMapper(IMapper<PhoneApplication, Phone> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public AddPhoneReplay Map(Result source)
        {
            var replay = new AddPhoneReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };

            if (source is OkResult<PhoneApplication> okResult)
            {
                replay.Value = _mapper.Map(okResult.Value);
            }

            return replay;
        }
    }
}
