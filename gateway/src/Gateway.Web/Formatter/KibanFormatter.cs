using Serilog.Formatting.Elasticsearch;

namespace Gateway.Web.Formatter
{
    public class KibanFormatter: ExceptionAsObjectJsonFormatter
    {
        public KibanFormatter()
            : base(renderMessage: true)
        {
            
        }
    }
}
