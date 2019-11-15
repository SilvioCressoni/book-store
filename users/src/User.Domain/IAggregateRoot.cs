using System;

namespace User.Domain
{
    public interface IAggregateRoot<TState, TId>
        where TState : IState<TId>
    {
        TState State { get; }

        void Apply<TEvent>(TEvent @event);
    }
}
