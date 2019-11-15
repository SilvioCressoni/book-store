using System;

namespace User.Domain.Event
{
    public class AddAddressEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public string Line { get; }
        public int Number { get; }
        public string PostCode { get; }

        public AddAddressEvent(string postCode, int number, string line)
        {
            PostCode = postCode ?? throw new ArgumentNullException(nameof(postCode));
            Number = number;
            Line = line ?? throw new ArgumentNullException(nameof(line));
        }
    }
}
