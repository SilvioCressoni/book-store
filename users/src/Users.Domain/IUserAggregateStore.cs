using System;

namespace Users.Domain
{
    public interface IUserAggregateStore : IAggregateStore<IUserAggregationRoot, UserState, Guid>
    {

    }
}
