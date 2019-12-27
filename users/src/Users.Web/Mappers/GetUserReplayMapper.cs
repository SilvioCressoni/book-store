using System;
using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Proto;
using UserApplication = Users.Application.Contracts.Response.User;

namespace Users.Web.Mappers
{
    public class GetUserReplayMapper : IMapper<Result, GetUserReplay>
    {
        private readonly IMapper<UserApplication, User> _mapper;

        public GetUserReplayMapper(IMapper<UserApplication, User> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public GetUserReplay Map(Result source)
        {
            var replay = new GetUserReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };

            if (source is OkResult<UserApplication> okResult)
            {
                replay.Value = _mapper.Map(okResult.Value);
            }

            return replay;
        }
    }
}
