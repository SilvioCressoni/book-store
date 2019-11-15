using System;

namespace User.Domain
{
    public interface IEvent
    {
        DateTime OccurredOn { get; }
    }
}
