using System;

namespace User.Domain.Event
{
    public class AddPhoneEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string Number { get; }

        public AddPhoneEvent(string number)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }
    }
}
