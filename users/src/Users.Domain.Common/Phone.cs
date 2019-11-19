using System;

namespace Users.Domain.Common
{
    public class Phone : IEquatable<Phone>
    {
        public virtual Guid Id { get; set; }
        public virtual string Number { get; set; }
        public virtual User User { get; set; }

        public virtual bool Equals(Phone other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            
            return Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is Phone phone)
            {
                return Equals(phone);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (Number != null ? Number.GetHashCode() : 0);
        }
    }
}