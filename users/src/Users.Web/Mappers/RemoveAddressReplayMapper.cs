using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Proto;

namespace Users.Web.Mappers
{
    public class RemoveAddressReplayMapper : IMapper<Result, RemoveAddressReplay>
    {
        public RemoveAddressReplay Map(Result source)
        {
            return new RemoveAddressReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };
        }
    }
}
