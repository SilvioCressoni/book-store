using Autofac;
using Gateway.API.Admin.Web.Headers;

namespace Gateway.API.Admin.Web.Modules
{
    public class HeaderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CorrelationId>()
                .As<ICorrelationId>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageId>()
                .As<IMessageId>()
                .InstancePerLifetimeScope();
        }
    }
}
