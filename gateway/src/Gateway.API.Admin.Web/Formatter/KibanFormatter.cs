using Serilog.Formatting.Elasticsearch;

namespace Gateway.API.Admin.Web.Formatter
{
    public class KibanFormatter : ExceptionAsObjectJsonFormatter
    {
        public KibanFormatter()
            : base(renderMessage: true)
        {
            
        }
    }
}
