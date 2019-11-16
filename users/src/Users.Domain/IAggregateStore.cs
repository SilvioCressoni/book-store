using System.Threading;
using System.Threading.Tasks;

namespace Users.Domain
{
    public interface IAggregateStore<TAggregateRoot, TState, TId>
        where  TAggregateRoot : IAggregateRoot<TState, TId>
        where TState : class, IState<TId>
    {
        TAggregateRoot Create();
        Task<TAggregateRoot> GetAsync(TId id, CancellationToken cancellation = default);
        Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellation = default);
    }
}
