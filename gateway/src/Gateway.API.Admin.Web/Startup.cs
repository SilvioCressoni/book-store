using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Gateway.API.Admin.Web.Interceptors;
using Gateway.API.Admin.Web.Modules;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Steeltoe.Common.Discovery;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul.Discovery;
using Steeltoe.Management.Endpoint.Health;

namespace Gateway.API.Admin.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            services.AddHealthActuator(Configuration);
            services.AddDiscoveryClient(Configuration);
            
            services.AddSingleton<IServiceInstanceProvider, ConsulDiscoveryClient>();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Book Store APIs";
                    document.Info.Description = "Microservice of Book Store";
                    document.Info.TermsOfService = "None";
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "MIT"
                    };
                };
            });
            
            var userService = Configuration.GetSection("Services")
                .Get<IEnumerable<Configuration.Service>>()
                .First(x=> x.Name == "User"); 
            
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", 
                Configuration.GetValue<bool>("Http2UnencryptedSupport"));
            
            services.AddGrpcClient<Users.Web.Proto.Users.UsersClient>("users", 
                    options =>
                    {
                        options.Address = new Uri(userService.Address);
                    })
                .AddInterceptor<AddRequestHeadersInterceptor>()
                .AddInterceptor<AddTraceHeadersInterceptor>()
                .ConfigureChannel(opt =>
                {
                    if(!userService.IsSecure)
                    {
                        opt.Credentials = ChannelCredentials.Insecure;
                    }
                })
                .AddServiceDiscovery()
                .AddRoundRobinLoadBalancer();
        }
        

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceModule>()
                .RegisterModule<CollectionModule>()
                .RegisterModule<HeaderModule>()
                .RegisterModule<InterceptorsModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseAuthorization();

            app.UseHealthActuator();
            app.UseDiscoveryClient();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
