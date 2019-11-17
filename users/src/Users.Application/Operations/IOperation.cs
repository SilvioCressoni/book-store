using System.Threading;
using System.Threading.Tasks;
using Users.Domain;

namespace Users.Application.Operations
{
    public interface IOperation<T>
    {
        ValueTask<Result> ExecuteAsync(T operation, CancellationToken cancellation = default);
    }
}
