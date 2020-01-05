using System;

namespace Gateway.API.Admin.Web.Headers
{
    public interface ICorrelationId
    {
        const string Headers = "X-Correlation-Id";
        string Id { get; }
    }
}
