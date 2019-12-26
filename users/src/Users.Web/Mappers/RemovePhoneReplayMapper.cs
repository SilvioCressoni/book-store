using Users.Application.Mapper;
using Users.Domain;

namespace Users.Web
{
    public class RemovePhoneReplayMapper : IMapper<Result, RemovePhoneReplay>
    {
        public RemovePhoneReplay Map(Result source)
        {
            return new RemovePhoneReplay
            {
                IsSuccess = false,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };
        }
    }
}
