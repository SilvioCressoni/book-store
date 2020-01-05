using System;

namespace Gateway.API.Admin.Web.Headers
{
    public class CorrelationId : ICorrelationId
    {
        public string Id { get; } = Guid.NewGuid().ToString();
    }
}
