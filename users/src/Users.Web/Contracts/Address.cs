using System;

namespace Users.Web.Contracts
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Line { get; set; }
        public int Number { get; set; }
        public string PostCode { get; set; }
    }
}
