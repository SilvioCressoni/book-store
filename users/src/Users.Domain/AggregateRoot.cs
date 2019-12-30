using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NHibernate;
using Users.Domain.Interceptors;

namespace Users.Domain
{
    public abstract class AggregateRoot<TState, TId> : IAggregateRoot<TState, TId>
        where TState : class, IState<TId>
    {
        public TState State { get; }

        private readonly IEnumerable<IAggregationRootInterceptor<TState, TId>> _interceptors;
        private readonly ILogger _logger;

        protected AggregateRoot(TState state, 
            IEnumerable<IAggregationRootInterceptor<TState, TId>> interceptors, 
            ILogger logger)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _interceptors = interceptors ?? throw new ArgumentNullException(nameof(interceptors));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Apply<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            OnApply(@event);
            ((dynamic)State).Apply(@event);
        }

        private void OnApply(IEvent @event)
        {
            try
            {
                foreach (var interceptor in _interceptors)
                {
                    interceptor.OnApply(State, @event);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error to Intercept");
            }
        }
    }
}
