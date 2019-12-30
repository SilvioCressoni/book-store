using System;
using System.Collections.Generic;

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
        {
            IObserver<IEvent> ob = null;
            try
            {
                ((dynamic)State).Apply(@event);
                ob.OnNext(@event);
            }
            catch (Exception e)
            {
                ob.OnError(e);
                throw;
            }
        }
    }
}
