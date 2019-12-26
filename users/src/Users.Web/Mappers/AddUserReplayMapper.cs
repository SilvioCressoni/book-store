using System;
using Users.Application.Mapper;
using Users.Domain;
using UserApplication = Users.Application.Contracts.Response.User;

namespace Users.Web.Mappers
{
    public class AddUserReplayMapper : IMapper<Result, AddUserReplay>
    {
        private readonly IMapper<UserApplication, User> _mapper;

        public AddUserReplayMapper(IMapper<UserApplication, User> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public AddUserReplay Map(Result source)
        {
            var replay = new AddUserReplay
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
