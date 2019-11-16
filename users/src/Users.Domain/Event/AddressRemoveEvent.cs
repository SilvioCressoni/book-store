using System;

namespace Users.Domain.Event
{
    public class AddressRemoveEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public Guid Id { get; }

        public AddressRemoveEvent(Guid id)
        {
            Id = id;
        }
    }
}
