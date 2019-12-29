using System;
using System.Linq;
using System.Text.RegularExpressions;
using Users.Domain.Event;
using Users.Infrastructure.Extensions;
using static Users.Domain.DomainError;

namespace Users.Domain
{
    public class UserAggregationRoot : AggregateRoot<UserState, Guid>, IUserAggregationRoot
    {
        public UserAggregationRoot(UserState state) 
            : base(state)
        {
        }

        #region Phone
        public Result AddPhone(string number)
        {
            if (number.IsMissing())
            {
                return PhoneError.MissingNumber;
            }

            if (number.Length > 15)
            {
                return PhoneError.InvalidNumber;
            }

            if (State.Phones.Any(x => x.Number == number))
            {
                return PhoneError.NumberAlreadyExist;
            }

            Apply(new PhoneAddEvent(number));
            return Result.Ok();
        }

        public Result RemovePhone(string number)
        {
            if (number.IsMissing())
            {
                return PhoneError.MissingNumber;
            }

            if (State.Phones.All(x => x.Number != number))
            {
                return PhoneError.NumberNotFound;
            }

            Apply(new PhoneRemoveEvent(number));
            return Result.Ok();
        }
        #endregion

        #region Address
        public Result AddAddress(string line, int number, string postCode)
        {
            if (line.IsMissing())
            {
                return AddressError.MissingLine;
            }

            if (line.Length > 100)
            {
                return AddressError.InvalidLine;
            }

            if (number <= 0)
            {
                return AddressError.InvalidNumber;
            }

            if (postCode.IsMissing())
            {
                return AddressError.MissingPostCode;
            }

            if (postCode.Length > 10)
            {
                return AddressError.InvalidPostCode;
            }

            if (State.Addresses.Any(x => x.Line == line && x.Number == number && x.PostCode == postCode))
            {
                return AddressError.AddressAlreadyExist;
            }

            Apply(new AddressAddEvent(postCode, number, line));
            return Result.Ok();
        }

        public Result RemoveAddress(Guid addressId)
        {
            if (addressId == Guid.Empty)
            {
                return AddressError.InvalidAddressId;
            }

            if (State.Addresses.All(x => x.Id != addressId))
            {
                return AddressError.AddressNotFound;
            }

            Apply(new AddressRemoveEvent(addressId));
            return Result.Ok();
        }
        #endregion

        private static readonly Regex emailValidator = new Regex(
            @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            RegexOptions.Compiled);

        public Result Create(string email, string firstName, string lastName, DateTime birthDay)
        {
            if (email.IsMissing())
            {
                return UserError.MissingEmail;
            }

            if (!emailValidator.IsMatch(email))
            {
                return UserError.InvalidEmail;
            }

            if (email.Length > 100)
            {
                return UserError.InvalidEmailLength;
            }

            if (firstName.IsMissing())
            {
                return UserError.MissingFirstName;
            }

            if (firstName.Length > 20)
            {
                return UserError.InvalidFirstName;
            }

            if (lastName.IsMissing())
            {
                return UserError.MissingLastNames;
            }

            if (lastName.Length > 100)
            {
                return UserError.InvalidLastNames;
            }

            Apply(new CreateEvent(email, firstName, lastName, birthDay));
            return Result.Ok();
        }

        public Result Update(string firstName, string lastName, DateTime birthDay)
        {
            if (firstName.IsMissing())
            {
                return UserError.MissingFirstName;
            }

            if (firstName.Length > 20)
            {
                return UserError.InvalidFirstName;
            }

            if (lastName.IsMissing())
            {
                return UserError.MissingLastNames;
            }

            if (lastName.Length > 100)
            {
                return UserError.InvalidLastNames;
            }

            Apply(new UpdateEvent(firstName, lastName, birthDay));
            return Result.Ok();
        }
    }
}