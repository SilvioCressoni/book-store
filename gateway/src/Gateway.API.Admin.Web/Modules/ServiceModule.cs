using Autofac;
using Gateway.API.Admin.Web.Collections;
using Gateway.Service;
using Grpc.Net.Client;

namespace Gateway.API.Admin.Web.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .WithParameter((parameter, _) => parameter.Name == "policy",
                    (_, context) => context.Resolve<IPolicyReadOnlyCollection>()["User"])
                .InstancePerLifetimeScope();
        }
    }
}
