using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;

namespace Gateway.API.Admin.Web.Collections
{
    public class PolicyReadOnlyCollection : IPolicyReadOnlyCollection
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMemoryCache _cache;
        private readonly Dictionary<string, (Configuration.Policy config, IAsyncPolicy policy)> _policies;
        private readonly Dictionary<string, ILogger> _loggers;

        public PolicyReadOnlyCollection(IMemoryCache cache, 
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            
            _policies = new Dictionary<string, (Configuration.Policy, IAsyncPolicy)>(StringComparer.OrdinalIgnoreCase);
            _loggers = new Dictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);
        }


        public IAsyncPolicy this[string service]
        {
            get
            {
                return _cache.GetOrCreate($"{service}-policy", entry =>
                {
                     var services = _configuration.GetSection("Services").Get<IEnumerable<Configuration.Service>>();
                     return GetPolicy(services.First(x => x.Name == service), GetLogger(service));
                 });
            }
        }

        private ILogger GetLogger(string service)
        {
            if (!_loggers.TryGetValue(service, out var logger))
            {
                _loggers.TryAdd(service, _loggerFactory.CreateLogger($"{service}.Policy"));
            }

            return logger;
        }

        private IAsyncPolicy GetPolicy(Configuration.Service service, ILogger logger)
        {
            if (!_policies.ContainsKey(service.Name))
            {
                _policies.Add(service.Name, (service.Policy, CreatePolicy(service, logger)));
            }
            else if(!service.Policy.Equals(_policies[service.Name].config))
            {
                _policies[service.Name] = (service.Policy, CreatePolicy(service, logger));
            }
            
            return _policies[service.Name].policy;
        }
        
        private static IAsyncPolicy CreatePolicy(Configuration.Service service, ILogger logger)
        {
            var policy = service.Policy;
            return Policy
                    .Handle<Exception>()
                    .AdvancedCircuitBreakerAsync(policy.FailureThreshold, policy.SamplingDuration, 
                        policy.MinimumThroughput, policy.DurationOfBreak,
                        onBreak: (exception, span, context) =>
                        {
                            logger.LogCritical(exception, "Circuit Breaker Active. [Name: {0}][Timespan: {1}]", service.Name, span);
                        },
                        onReset: context =>
                        {
                            logger.LogInformation("Circuit Breaker reset. [Name: {0}][DateTime: {1}]", service.Name,
                                DateTime.UtcNow);
                        });
        }
    }
}
