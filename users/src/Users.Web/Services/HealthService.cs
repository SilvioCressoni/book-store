using System;
using System.Threading.Tasks;
using Grpc.Core;
using Steeltoe.Common.HealthChecks;
using Users.Web.Proto;

namespace Users.Web.Services
{
    public class HealthService : Health.HealthBase
    {
        private readonly IHealthContributor _health;

        public HealthService(IHealthContributor health)
        {
            _health = health ?? throw new ArgumentNullException(nameof(health));
        }

        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            var health = _health.Health();

            if (_health.Id == request.Service)
            {
                return Task.FromResult(new HealthCheckResponse
                {
                    Status = Map(health.Status)
                });
            }
            else if (health.Details.TryGetValue(request.Service, out var status))
            {
                return Task.FromResult(new HealthCheckResponse
                {
                    Status = Map(status)
                });
            }

            return Task.FromResult(new HealthCheckResponse
            {
                Status = HealthCheckResponse.Types.ServingStatus.Unknown
            });
        }

        private static HealthCheckResponse.Types.ServingStatus Map(HealthStatus status)
        {
            switch (status)
            {
                case HealthStatus.UNKNOWN:
                    return HealthCheckResponse.Types.ServingStatus.Unknown;
                case HealthStatus.UP:
                    return HealthCheckResponse.Types.ServingStatus.Serving;
                case HealthStatus.WARNING:
                    return HealthCheckResponse.Types.ServingStatus.Serving;
                case HealthStatus.OUT_OF_SERVICE:
                    return HealthCheckResponse.Types.ServingStatus.NotServing;
                case HealthStatus.DOWN:
                    return HealthCheckResponse.Types.ServingStatus.NotServing;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        private static HealthCheckResponse.Types.ServingStatus Map(object status)
        {
            switch (status)
            {
                case HealthStatus healthStatus:
                    return Map(healthStatus);
                case string stringStatus:
                    if (Enum.TryParse(stringStatus, true, out HealthStatus s))
                    {
                        return Map(s);
                    }
                    goto default;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public override Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context)
        {
            return base.Watch(request, responseStream, context);
        }
    }
}
