using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace Gateway.API.Admin.Web.Interceptors
{
    public class AddRequestHeadersInterceptor : Interceptor
    {
        private readonly IHttpContextAccessor _context;
        private readonly string[] _headerToFlow = new[]
        {
            "X-Party-Id"
        };

        public AddRequestHeadersInterceptor(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            foreach (var header in _headerToFlow)
            {
                if (_context.HttpContext.Request.Headers.TryGetValue(header, out var value))
                {
                    context.Options.Headers.Add(header, value);
                }
            }
            
            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
