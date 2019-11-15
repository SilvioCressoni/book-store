using System;

namespace User.Domain
{
    public interface IUserAggregateStore : IAggregateStore<IUserAggregationRoot, UserState, Guid>
    {

    }
}
