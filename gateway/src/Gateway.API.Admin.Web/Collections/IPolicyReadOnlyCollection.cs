using Polly;

namespace Gateway.API.Admin.Web.Collections
{
    public interface IPolicyReadOnlyCollection
    {
        IAsyncPolicy this[string service] { get; }
    }
}
