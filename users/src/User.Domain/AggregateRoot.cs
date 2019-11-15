using System;

namespace User.Domain
{
    public abstract class AggregateRoot<T> : IAggregateRoot<T>
        where  T : class
    {
        public DateTime? LastUpdateDate { get; set; }
        public T State { get; }

        protected AggregateRoot(T state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
        }

        public void Apply<TEvent>(TEvent @event)
            => ((dynamic) State).Apply(@event);
    }
}
