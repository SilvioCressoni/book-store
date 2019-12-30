using System;
using System.Collections.Generic;

namespace Users.Domain
{
    public abstract class AggregateRoot<TState, TId> : IAggregateRoot<TState, TId>
        where TState : class, IState<TId>
    {
        private readonly ICollection<IEvent> _events;
        public TState State { get; }
        public IEnumerable<IEvent> Events => _events;

        protected AggregateRoot(TState state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _events = new List<IEvent>(1);
        }

        public void AddEvent(IEvent @event)
            => _events.Add(@event);

        public void Apply(IEvent @event)
        {
            State.Apply(@event);
            _events.Add(@event);
        }
    }
}
