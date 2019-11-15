using System;

namespace User.Domain
{
    public class UserState
    {
        public bool ContainPhone(string number)
            => false;

        public bool ContainAddress(Guid addressId)
            => false;

        public bool ContainAddress(string line, int number, string postCode)
            => false;
    }
}
