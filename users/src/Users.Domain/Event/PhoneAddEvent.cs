using System;

namespace Users.Domain.Event
{
    public class PhoneAddEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string Number { get; }

        public PhoneAddEvent(string number)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }
    }
}
