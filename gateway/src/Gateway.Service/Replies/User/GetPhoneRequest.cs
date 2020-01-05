using System.Collections.Generic;
using System.Linq;

namespace Users.Web.Proto
{
    public partial class GetPhoneReplay : IReply<IEnumerable<Phone>>
    {
        IEnumerable<Phone> IReply<IEnumerable<Phone>>.Value => Value.AsEnumerable();
    }
}
