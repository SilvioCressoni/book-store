using Autofac;
using Gateway.API.Admin.Web.Collections;

namespace Gateway.API.Admin.Web.Modules
{
    public class CollectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PolicyReadOnlyCollection>()
                .As<IPolicyReadOnlyCollection>()
                .AsSelf();
        }
    }
}
