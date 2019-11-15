using System;

namespace User.Domain
{
    public interface IUserAggregation
    {
        void AddPhone(string number);
        void RemovePhone(string number);
        
        void AddAddress(string line, int number, string postCode);
        void RemoveAddress(Guid addressId);

        void UpdateUser(string firstName, string lastName, DateTime birthDay);
    }
}
