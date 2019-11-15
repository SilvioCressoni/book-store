using System;
using System.Linq;
using User.Domain.Common;

namespace User.Domain
{
    public class UserAggregation : IUserAggregation
    {
        private readonly Common.User _user;

        public UserAggregation(Common.User user)
        {
            _user = user;
        }

        public void AddPhone(string number)
        {
            _user.Phones.Add(new Phone
            {
                Number = number
            });
        }

        public void RemovePhone(string number)
        {
            _user.Phones.Remove(_user.Phones.First(x => x.Number == number));
        }

        public void AddAddress(string line, int number, string postCode)
        {
            _user.Addresses.Add(new Address
            {
                Line = line,
                Number = number,
                PostCode = postCode
            });
        }

        public void RemoveAddress(Guid addressId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string firstName, string lastName, DateTime birthDay)
        {
            _user.FirstName = firstName;
            _user.LastNames = lastName;
            _user.BirthDay = birthDay;
        }
    }
}