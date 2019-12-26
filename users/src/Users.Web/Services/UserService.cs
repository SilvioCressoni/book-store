using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Users.Application.Contracts;
using Users.Application.Contracts.Request;
using Users.Application.Mapper;
using Users.Application.Operations;
using Users.Domain;

namespace Users.Web.Services
{
    public class UserService : Users.UsersBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly IServiceProvider _provider;

        public UserService(ILogger<UserService> logger, IServiceProvider provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        #region Phone

        public override async Task<GetPhoneReplay> GetPhones(GetPhoneRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<PhoneGetOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, GetPhoneReplay>>();
            if (Guid.TryParse(request.UserId, out var userId))
            {
                var result = await operation.ExecuteAsync(new PhoneGet
                {
                    UserId = userId
                });

                return mapper.Map(result);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        public override async Task<AddPhoneReplay> AddPhone(AddPhoneRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<PhoneAddOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, AddPhoneReplay>>();
            if (Guid.TryParse(request.UserId, out var userId))
            {
                var result = await operation.ExecuteAsync(new PhoneAdd
                {
                    UserId = userId,
                    Number = request.Number
                });

                return mapper.Map(result);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        public override async Task<RemovePhoneReplay> RemovePhone(RemovePhoneRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<PhoneRemoveOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, RemovePhoneReplay>>();
            if (Guid.TryParse(request.UserId, out var userId))
            {
                var result = await operation.ExecuteAsync(new PhoneRemove
                {
                    UserId = userId
                });

                return mapper.Map(result);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        #endregion

        #region Address

        public override async Task<GetAddressesReplay> GetAddresses(GetAddressesRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<AddressGetOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, GetAddressesReplay>>();
            if (Guid.TryParse(request.UserId, out var userId))
            {
                var result = await operation.ExecuteAsync(new AddressGet
                {
                    UserId = userId
                });

                return mapper.Map(result);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        public override async Task<AddAddressReplay> AddAddress(AddAddressRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<AddressAddOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, AddAddressReplay>>();
            if (Guid.TryParse(request.UserId, out var userId))
            {
                var result = await operation.ExecuteAsync(new AddressAdd
                {
                    UserId = userId,
                    Number = request.Number,
                    Line = request.Line,
                    PostCode = request.PostCode
                });

                return mapper.Map(result);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        public override async Task<RemoveAddressReplay> RemoveAddress(RemoveAddressRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<AddressRemoveOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, RemoveAddressReplay>>();
            if (Guid.TryParse(request.UserId, out var userId))
            {
                if (Guid.TryParse(request.Id, out var id))
                {
                    var result = await operation.ExecuteAsync(new AddressRemove
                    {
                        UserId = userId,
                        Id = id,
                    });

                    return mapper.Map(result);
                }

                return mapper.Map(DomainError.UserError.InvalidUserId);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        #endregion

        #region User

        public override async Task<AddUserReplay> AddUsers(AddUserRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<UserCreateOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, AddUserReplay>>();

            var result = await operation.ExecuteAsync(new UserAdd
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastNames = request.LastNames,
                BirthDate = request.BirthDate.ToDateTime()
            });

            return mapper.Map(result);
        }


        public override async Task<GetUserReplay> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<UserGetOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, GetUserReplay>>();
            if (Guid.TryParse(request.UserId, out var userId))
            {
                var result = await operation.ExecuteAsync(new UserGet
                {
                    Id = userId,
                });

                return mapper.Map(result);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        public override async Task<UpdateUserReplay> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var operation = _provider.GetRequiredService<UserUpdateOperation>();
            var mapper = _provider.GetRequiredService<IMapper<Result, UpdateUserReplay>>();
            if (Guid.TryParse(request.Id, out var userId))
            {
                var result = await operation.ExecuteAsync(new UserUpdate
                {
                    Id = userId,
                    FirstName = request.FirstName,
                    LastNames = request.LastNames,
                    BirthDate = request.BirthDate.ToDateTime()
                });

                return mapper.Map(result);
            }

            return mapper.Map(DomainError.UserError.InvalidUserId);
        }

        #endregion
    }
}
