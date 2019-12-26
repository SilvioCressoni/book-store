using Autofac;
using Users.Domain;

namespace Users.Web.Modules
{
    public class AggregationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserAggregateStore>()
                .As<IUserAggregateStore>()
                .InstancePerLifetimeScope();
        }
    }
}
