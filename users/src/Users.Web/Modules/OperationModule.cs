using Autofac;
using Users.Application.Operations;

namespace Users.Web.Modules
{
    public class OperationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PhoneAddOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<PhoneRemoveOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<PhoneGetOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<AddressAddOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<AddressRemoveOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<AddressGetOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserCreateOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserUpdateOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserGetOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserGetAllOperation>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
