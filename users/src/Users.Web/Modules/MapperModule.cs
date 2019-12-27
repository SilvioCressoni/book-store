using Autofac;
using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Mappers;
using Users.Web.Proto;
using AddressResponse = Users.Application.Contracts.Response.Address;
using PhoneResponse = Users.Application.Contracts.Response.Phone;
using UserResponse = Users.Application.Contracts.Response.User;

namespace Users.Web.Modules
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddressMapper>()
                .As<IMapper<Domain.Common.Address, AddressResponse>>()
                .SingleInstance();

            builder.RegisterType<PhoneMapper>()
                .As<IMapper<Domain.Common.Phone, PhoneResponse>>()
                .SingleInstance();

            builder.RegisterType<UserMapper>()
                .As<IMapper<Domain.Common.User, UserResponse>>()
                .SingleInstance();

            #region Grpc Mapper

            #region Phone

            builder.RegisterType<GrpcPhoneMapper>()
                .As<IMapper<PhoneResponse, Phone>>()
                .SingleInstance();

            builder.RegisterType<GetPhoneReplayMapper>()
                .As<IMapper<Result, GetPhoneReplay>>()
                .SingleInstance();

            builder.RegisterType<AddPhoneReplayMapper>()
                .As<IMapper<Result, AddPhoneReplay>>()
                .SingleInstance();

            builder.RegisterType<RemovePhoneReplayMapper>()
                .As<IMapper<Result, RemovePhoneReplay>>()
                .SingleInstance();

            #endregion

            #region Address

            builder.RegisterType<GrpcAddressMapper>()
                .As<IMapper<AddressResponse, Address>>()
                .SingleInstance();

            builder.RegisterType<GetAddressesReplayMapper>()
                .As<IMapper<Result, GetAddressesReplay>>()
                .SingleInstance();

            builder.RegisterType<AddAddressReplayMapper>()
                .As<IMapper<Result, AddAddressReplay>>()
                .SingleInstance();

            builder.RegisterType<RemoveAddressReplayMapper>()
                .As<IMapper<Result, RemoveAddressReplay>>()
                .SingleInstance();

            #endregion

            #region User

            builder.RegisterType<GrpcUserMapper>()
                .As<IMapper<UserResponse, User>>()
                .SingleInstance();

            builder.RegisterType<AddUserReplayMapper>()
                .As<IMapper<Result, AddUserReplay>>()
                .SingleInstance();

            builder.RegisterType<GetUserReplayMapper>()
                .As<IMapper<Result, GetUserReplay>>()
                .SingleInstance();

            builder.RegisterType<UpdateUserReplayMapper>()
                .As<IMapper<Result, UpdateUserReplay>>()
                .SingleInstance();

            #endregion

            #endregion
        }
    }
}
