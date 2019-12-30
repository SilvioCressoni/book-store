using System.Collections.Generic;

namespace Users.Domain
{
    public interface IAggregateRoot<TState, TId>
        where TState : class, IState<TId>
    {
        TState State { get; }
        void Apply<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}
