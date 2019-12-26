using Autofac;
using Users.Application.Mapper;
using Users.Web.Mappers;

namespace Users.Web.Modules
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddressMapper>()
                .As<IMapper<Domain.Common.Address, Address>>()
                .SingleInstance();

            builder.RegisterType<PhoneMapper>()
                .As<IMapper<Domain.Common.Phone, Phone>>()
                .SingleInstance();

            builder.RegisterType<UserMapper>()
                .As<IMapper<Domain.Common.User, User>>()
                .SingleInstance();

            #region Grpc Mapper


            #endregion
        }
    }
}
