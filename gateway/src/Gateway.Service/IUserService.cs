using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gateway.Service
{
    public interface IUserService
    {
        Task<IStatusCodeResult> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<IStatusCodeResult> AddUserAsync(string email, string firstName, string lastName, DateTime birthDate, CancellationToken cancellationToken = default);
        Task<IStatusCodeResult> UpdateUserAsync(string id, string firstName, string lastName, DateTime birthDate, CancellationToken cancellationToken = default);

        #region Phone
        Task<IStatusCodeResult> GetPhonesAsync(string userId, CancellationToken cancellationToken = default);
        Task<IStatusCodeResult> AddPhoneAsync(string userId, string number, CancellationToken cancellationToken = default);
        Task<IStatusCodeResult> RemovePhoneAsync(string userId, string number, CancellationToken cancellationToken = default);
        #endregion

        #region Address

        Task<IStatusCodeResult> GetAddressesAsync(string userId, CancellationToken cancellationToken = default);
        Task<IStatusCodeResult> AddAddressAsync(string userId, string line, int number, string postCode, CancellationToken cancellationToken = default);
        Task<IStatusCodeResult> RemoveAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default);

        #endregion
    }
}
