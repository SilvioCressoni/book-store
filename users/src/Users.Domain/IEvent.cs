using System;

namespace Users.Domain
{
    public interface IEvent
    {
        DateTime OccurredOn { get; }
    }
}
