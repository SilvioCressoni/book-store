using System;
using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Proto;
using UserApplication = Users.Application.Contracts.Response.User;

namespace Users.Web.Mappers
{
    public class UpdateUserReplayMapper : IMapper<Result, UpdateUserReplay>
    {
        private readonly IMapper<UserApplication, User> _mapper;

        public UpdateUserReplayMapper(IMapper<UserApplication, User> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public UpdateUserReplay Map(Result source)
        {
            var replay = new UpdateUserReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description ?? string.Empty,
                ErrorCode = source.ErrorCode ?? string.Empty
            };

            if (source is OkResult<UserApplication> okResult)
            {
                replay.Value = _mapper.Map(okResult.Value);
            }

            return replay;
        }
    }
}
