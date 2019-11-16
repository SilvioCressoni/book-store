using System.Threading;
using System.Threading.Tasks;
using Users.Domain;

namespace Users.Application.Operations
{
    public interface IOperation<T>
    {
        Task<Result> ExecuteAsync(T operation, CancellationToken cancellation = default);
    }
}
