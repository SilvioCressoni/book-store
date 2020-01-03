using System.Net;

namespace Gateway.Service
{
    public class StatusCodeResult
    {
        public StatusCodeResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Value = null;
        }

        public StatusCodeResult(HttpStatusCode statusCode, object value)
        {
            StatusCode = statusCode;
            Value = value;
        }

        public HttpStatusCode StatusCode { get; }
        public object Value { get; }
    }

    public class StatusCodeResult<T> : StatusCodeResult
    {
        public StatusCodeResult(HttpStatusCode statusCode)
            : base(statusCode)
        {
            Value = default;
        }

        public StatusCodeResult(HttpStatusCode statusCode, T value)
            : base(statusCode, value)
        {
            Value = value;
        }

        public new T Value { get; }
    }
}
