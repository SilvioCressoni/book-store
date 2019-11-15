using System;

namespace User.Domain
{
    public interface IUserAggregationRoot : IAggregateRoot<UserState>
    {
        #region Phone
        Result AddPhone(string number);
        Result RemovePhone(string number);
        #endregion

        #region Address
        Result AddAddress(string line, int number, string postCode);
        Result RemoveAddress(Guid addressId);
        #endregion

        Result Create(string email, string firstName, string lastName, DateTime birthDay);
        Result Update(string firstName, string lastName, DateTime birthDay);
    }
}
