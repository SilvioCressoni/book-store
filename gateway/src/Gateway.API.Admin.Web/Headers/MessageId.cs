using System;
using System.Collections.Generic;

namespace Gateway.API.Admin.Web.Headers
{
    public class MessageId : IMessageId
    {
        private readonly LinkedList<string> _ids = new LinkedList<string>();
        public IEnumerable<string> Ids => _ids;
        
        public string Create()
        {
            var guid = Guid.NewGuid().ToString();
            _ids.AddLast(guid);
            return guid;
        }
    }
}
