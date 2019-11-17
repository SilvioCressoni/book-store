using System;

namespace Users.Application.Contracts.Request
{
    public class AddressAdd
    {
        public Guid UserId { get; set; }
        public string Line { get; set; }
        public int Number { get; set; }
        public string PostCode { get; set; }
    }
}
