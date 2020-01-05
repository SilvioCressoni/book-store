using System.Collections.Generic;
using System.Linq;

namespace Users.Web.Proto
{
    public partial class GetAddressesReplay : IReply<IEnumerable<Address>>
    {
        IEnumerable<Address> IReply<IEnumerable<Address>>.Value => Value.AsEnumerable();
    }
}
