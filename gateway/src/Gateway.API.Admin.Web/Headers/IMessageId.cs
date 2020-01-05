using System;
using System.Collections.Generic;

namespace Gateway.API.Admin.Web.Headers
{
    public interface IMessageId
    {
        const string Header = "X-Message-Id";
        IEnumerable<string> Ids { get; }
        string Create();
    }
}
