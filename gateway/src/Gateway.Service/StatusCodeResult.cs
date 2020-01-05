using System.Net;

namespace Gateway.Service
{
    public interface IStatusCodeResult
    {
        HttpStatusCode StatusCode { get; }
        object Value { get; }
        
        public static IStatusCodeResult Ok<T>(T value)
            => new StatusCodeResult<T>(HttpStatusCode.OK, value);
        
        public static IStatusCodeResult NoContent()
            => new StatusCodeResult<object>(HttpStatusCode.NoContent, default);
        
        public static IStatusCodeResult UnprocessableEntity<T>(T value)
            => new StatusCodeResult<T>(HttpStatusCode.UnprocessableEntity, value);
        
        public static IStatusCodeResult BadRequest<T>(T value)
            => new StatusCodeResult<T>(HttpStatusCode.BadRequest, value);
        
        public static IStatusCodeResult NotFound<T>(T value)
            => new StatusCodeResult<T>(HttpStatusCode.NotFound, value);
    }
    
    public interface IStatusCodeResult<T> : IStatusCodeResult
    {
        new T Value { get; }
        object IStatusCodeResult.Value => Value;
    }
    
    public class StatusCodeResult<T> : IStatusCodeResult<T>
    {
        public StatusCodeResult(HttpStatusCode statusCode, T value)

        {
            StatusCode = statusCode;
            Value = value;
        }

        public HttpStatusCode StatusCode { get; }
        public T Value { get; }
    }
}
