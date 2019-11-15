using System;

namespace User.Domain
{
    public interface IAggregateRoot<T>
        where  T : class
    {
        DateTime? LastUpdateDate { get; set; }

        T State { get; }

        void Apply<TEvent>(TEvent @event);
    }
}
