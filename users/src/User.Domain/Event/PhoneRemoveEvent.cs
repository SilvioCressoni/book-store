using System;

namespace User.Domain.Event
{
    public class PhoneRemoveEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string Number { get; }

        public PhoneRemoveEvent(string number)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }
    }
}
