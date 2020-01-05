using System;
using Gateway.API.Admin.Web.Headers;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Gateway.API.Admin.Web.Interceptors
{
    public class AddTraceHeadersInterceptor : Interceptor
    {
        private readonly ICorrelationId _correlationId;
        private readonly IMessageId _messageId;

        public AddTraceHeadersInterceptor(IMessageId messageId, ICorrelationId correlationId)
        {
            _messageId = messageId ?? throw new ArgumentNullException(nameof(messageId));
            _correlationId = correlationId ?? throw new ArgumentNullException(nameof(correlationId));
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            context.Options.Headers.Add(ICorrelationId.Headers, _correlationId.Id);
            context.Options.Headers.Add(IMessageId.Header, _messageId.Create());
            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
