using System.Threading;
using System.Threading.Tasks;

namespace User.Domain
{
    public interface IAggregateStore<TAggregateRoot, TState, TId>
        where  TAggregateRoot : IAggregateRoot<TState, TId>
        where TState : IState<TId>
    {
        TAggregateRoot Create();
        Task<TAggregateRoot> GetAsync(TId id, CancellationToken cancellation = default);
        Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellation = default);
    }
}
