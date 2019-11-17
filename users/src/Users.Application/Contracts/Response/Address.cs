using System;

namespace Users.Application.Contracts.Response
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Line { get; set; }
        public int Number { get; set; }
        public string PostCode { get; set; }
    }
}
