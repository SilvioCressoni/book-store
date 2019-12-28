using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Proto;

namespace Users.Web.Mappers
{
    public class RemovePhoneReplayMapper : IMapper<Result, RemovePhoneReplay>
    {
        public RemovePhoneReplay Map(Result source)
        {
            return new RemovePhoneReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description ?? string.Empty,
                ErrorCode = source.ErrorCode ?? string.Empty
            };
        }
    }
}
