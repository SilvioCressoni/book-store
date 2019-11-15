using System;
using System.Text.RegularExpressions;
using User.Domain.Event;

using static User.Domain.DomainError;

namespace User.Domain
{
    public class UserAggregationRoot : AggregateRoot<UserState>, IUserAggregationRoot
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
                return PhoneError.InvalidNumber;
            }

            if (State.ContainPhone(number))
            {
                return PhoneError.NumberAlreadyExist;
            }

            Apply(new AddPhoneEvent(number));
            return Result.Ok;
        }

        public Result RemovePhone(string number)
        {
            if (number.IsMissing())
            {
                return PhoneError.InvalidNumber;
            }

            if (!State.ContainPhone(number))
            {
                return PhoneError.NumbernotFound;
            }

            Apply(new RemovePhoneEvent(number));
            return Result.Ok;
        }
        #endregion

        #region Address
        public Result AddAddress(string line, int number, string postCode)
        {
            if (line.IsMissing())
            {
                return AddressError.InvalidLine;
            }

            if (number <= 0)
            {
                return AddressError.InvalidNumber;
            }

            if (postCode.IsMissing())
            {
                return AddressError.InvalidPostCode;
            }

            if (State.ContainAddress(line, number, postCode))
            {
                return AddressError.AddressAlreadyExist
                    ;
            }

            Apply(new AddAddressEvent(postCode, number, line));
            return Result.Ok;
        }

        public Result RemoveAddress(Guid addressId)
        {
            if (addressId == Guid.Empty)
            {
                return AddressError.InvalidAddressId;
            }

            if (!State.ContainAddress(addressId))
            {
                return AddressError.AddressNotFound;
            }

            Apply(new RemoveAddressEvent(addressId));
            return Result.Ok;
        }
        #endregion

        private static readonly Regex emailValidator = new Regex(
            @"/^(([^<>()\[\]\\.,;:\s@""]+ (\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/",
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

            if (firstName.IsMissing())
            {
                return UserError.MissingFirstName;
            }

            if (firstName.Length > 100)
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

            var year = DateTime.Now.Year - birthDay.Year;
            if (year < 18)
            {
                return UserError.InvalidBirthDay;
            }

            Apply(new CreateUserEvent(email, firstName, lastName, birthDay));
            return Result.Ok;
        }

        public Result Update(string firstName, string lastName, DateTime birthDay)
        {
            if (firstName.IsMissing())
            {
                return UserError.MissingFirstName;
            }

            if (firstName.Length > 100)
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

            var year = DateTime.Now.Year - birthDay.Year;
            if (year < 18)
            {
                return UserError.InvalidBirthDay;
            }

            Apply(new UpdateUserEvent(firstName, lastName, birthDay));
            return Result.Ok;
        }
    }
}