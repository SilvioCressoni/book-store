using System.Collections.Generic;

namespace Users.Domain
{
    public interface IAggregateRoot<TState, TId>
        where TState : class, IState<TId>
    {
        TState State { get; }

        IEnumerable<IEvent> Events { get; }

        void AddEvent(IEvent @event);

        void Apply(IEvent @event);
    }
}
