using Users.Domain;
using Users.Web.Replay;

namespace Users.Web.Mapper
{
    internal static class ResultMapper
    {
        public static T Map<T>(Result result)
            where T : IReplay, new()
        {
            var ret = new T
            {
                IsSuccess = result.IsSuccess, 
                Description = result.Description, 
                ErrorCode = result.ErrorCode,
                Value = result.Value
            };

            return ret;
        }
    }
}
