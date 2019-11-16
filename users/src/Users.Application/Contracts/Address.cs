using System;

namespace Users.Application.Contracts
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Line { get; set; }
        public int Number { get; set; }
        public string PostCode { get; set; }
    }
}
