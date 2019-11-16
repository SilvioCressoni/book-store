using System;

namespace Users.Domain.Event
{
    public class AddressAddEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public string Line { get; }
        public int Number { get; }
        public string PostCode { get; }

        public AddressAddEvent(string postCode, int number, string line)
        {
            PostCode = postCode ?? throw new ArgumentNullException(nameof(postCode));
            Number = number;
            Line = line ?? throw new ArgumentNullException(nameof(line));
        }
    }
}
