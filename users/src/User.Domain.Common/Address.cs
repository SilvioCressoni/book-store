using System;

namespace User.Domain.Common
{
    public class Address
    {
        public virtual Guid Id { get; set; }
        public virtual string Line { get; set; }
        public virtual int Number { get; set; }
        public virtual string PostCode { get; set; }
        
        public virtual User User { get; set; }
    }
}