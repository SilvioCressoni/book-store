using System;

namespace Users.Domain
{
    public abstract class AggregateRoot<TState, TId> : IAggregateRoot<TState, TId>
        where TState : class, IState<TId>
    {
        public TState State { get; }

        protected AggregateRoot(TState state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
        }

        public void Apply<TEvent>(TEvent @event)
            where TEvent : IEvent 
            => ((dynamic)State).Apply(@event);
    }
}
