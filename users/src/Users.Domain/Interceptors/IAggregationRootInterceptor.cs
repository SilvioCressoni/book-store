namespace Users.Domain.Interceptors
{
    public interface IAggregationRootInterceptor<TState, TId>
        where TState : class, IState<TId>
    {
        void OnApply(TState state, IEvent @event);
    }
}
