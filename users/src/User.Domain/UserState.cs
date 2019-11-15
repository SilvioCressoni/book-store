using System;
using System.Collections.Generic;
using System.Linq;
using User.Domain.Common;
using User.Domain.Event;

namespace User.Domain
{
    public class UserState
    {
        private readonly Common.User _user;

        public UserState(Common.User user)
        {
            _user = user;
        }

        public bool ContainPhone(string number) 
            => _user.Phones.Any(x => x.Number == number);

        public bool ContainAddress(Guid addressId)
            => _user.Addresses.Any(x => x.Id == addressId);

        public bool ContainAddress(string line, int number, string postCode)
            => _user.Addresses.Any(x=> x.Line == line && x.Number == number && x.PostCode == postCode);

        public void Apply(PhoneAddEvent @event)
        {
            _user.Phones.Add(new Phone
            {
                Number = @event.Number
            });
        }

        public void Apply(PhoneRemoveEvent @event)
        {
            _user.Phones.Remove(x => x.Number == @event.Number);
        }

        public void Apply(AddressAddEvent @event)
        {
            _user.Addresses.Add(new Address
            {
                Line = @event.Line,
                Number = @event.Number,
                PostCode =  @event.PostCode
            });
        }

        public void Apply(AddressRemoveEvent @event)
        {
            _user.Addresses.Remove(x => x.Id == @event.Id);
        }

        public void Apply(UpdateEvent @event)
        {
            _user.FirstName = @event.FirstName;
            _user.LastNames = @event.LastNames;
            _user.BirthDay = @event.BirthDay;
        }

        public void Apply(CreateEvent @event)
        {
            _user.Email = @event.Email;
            _user.FirstName = @event.FirstName;
            _user.LastNames = @event.LastNames;
            _user.BirthDay = @event.BirthDay;
        }
    }
}
