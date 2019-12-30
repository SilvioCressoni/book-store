using System;
using System.Collections.Generic;
using System.Linq;
using Users.Domain.Common;
using Users.Domain.Event;
using Users.Infrastructure.Extensions;

namespace Users.Domain
{
    public class UserState : IState<Guid>
    {
        private readonly Common.User _user;

        public Guid Id => _user.Id;
        public string Email => _user.Email;
        public string FirstName => _user.FirstName;
        public string LastNames => _user.LastNames;
        public DateTime BirthDay => _user.BirthDay;
        public IEnumerable<Phone> Phones => _user.Phones;
        public IEnumerable<Address> Addresses => _user.Addresses;

        public UserState(Common.User user)
        {
            _user = user;
        }

        public void Apply(IEvent @event)
        {
            switch (@event)
            {
                case PhoneAddEvent addPhone:
                    Apply(addPhone);
                    break;
                case PhoneRemoveEvent removePhone:
                    Apply(removePhone);
                    break;
                case AddressAddEvent addressAddEvent:
                    Apply(addressAddEvent);
                    break;
                case AddressRemoveEvent addressRemoveEvent:
                    Apply(addressRemoveEvent);
                    break;
                default:
                    throw new ArgumentException();
            }
        }
        
        private void Apply(PhoneAddEvent @event)
        {
            _user.Phones.Add(new Phone
            {
                Number = @event.Number,
                User = _user
            });
        }

        private void Apply(PhoneRemoveEvent @event) 
            => _user.Phones.Remove(x => x.Number == @event.Number);

        public void Apply(AddressAddEvent @event)
        {
            _user.Addresses.Add(new Address
            {
                Line = @event.Line,
                Number = @event.Number,
                PostCode =  @event.PostCode,
                User = _user
            });
        }

        public void Apply(AddressRemoveEvent @event) 
            => _user.Addresses.Remove(x => x.Id == @event.Id);

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

        public static explicit operator Common.User(UserState state)
            => state._user;
    }
}
