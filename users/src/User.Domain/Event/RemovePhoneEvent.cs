using System;

namespace User.Domain.Event
{
    public class RemovePhoneEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string Number { get; }

        public RemovePhoneEvent(string number)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }
    }
}
