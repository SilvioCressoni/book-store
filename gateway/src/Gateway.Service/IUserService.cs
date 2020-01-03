using System.Threading.Tasks;

namespace Gateway.Service
{
    public interface IUserService
    {
        Task<StatusCodeResult> GetAsync(string id);
    }
}
