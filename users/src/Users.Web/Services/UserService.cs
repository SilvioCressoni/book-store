using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

    }
}
