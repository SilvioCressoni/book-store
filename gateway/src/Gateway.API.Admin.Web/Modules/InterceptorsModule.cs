using Autofac;
using Gateway.API.Admin.Web.Interceptors;

namespace Gateway.API.Admin.Web.Modules
{
    public class InterceptorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddTraceHeadersInterceptor>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<AddRequestHeadersInterceptor>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
