using System;

namespace User.Domain.Event
{
    public class RemoveAddressEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public Guid Id { get; }

        public RemoveAddressEvent(Guid id)
        {
            Id = id;
        }
    }
}
