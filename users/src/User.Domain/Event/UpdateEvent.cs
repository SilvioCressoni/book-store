using System;

namespace User.Domain.Event
{
    public class UpdateEvent : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string FirstName { get; }
        public string LastNames { get; }
        public DateTime BirthDay { get; }

        public UpdateEvent(string firstName, string lastNames, DateTime birthDay)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastNames = lastNames ?? throw new ArgumentNullException(nameof(lastNames));
            BirthDay = birthDay;
        }
    }
}
