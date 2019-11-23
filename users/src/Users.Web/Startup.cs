using System.Collections.Generic;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using NHibernate.Cfg;
using NJsonSchema;
using Npgsql;
using NSwag;
using NSwag.Generation;
using Users.Application.Contracts.Response;
using Users.Application.Mapper;
using Users.Application.Operations;
using Users.Domain;
using Users.Infrastructure;
using Users.Infrastructure.Mapper;

namespace Users.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();
            services.AddSwaggerDocument(config => { 
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Users API";
                    document.Info.Description = "User Manager";
                    document.Info.TermsOfService = "None";
                    document.Schemes = new List<OpenApiSchema>
                    {
                        OpenApiSchema.Http,
                        OpenApiSchema.Https
                    };
                };
            });
            //services.AddGrpc();

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
            });
        }
        
        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(provider => Fluently.Configure(new Configuration()
                        .SetNamingStrategy(new PostgresNamingStrategy()))
                .Database(() =>
                {
                    var configure = provider.Resolve<IConfiguration>();
                    var connection = configure.GetConnectionString("Postgres");
                    
                    var builder = new NpgsqlConnectionStringBuilder(connection)
                    {
                        ApplicationName = "BookStoreUser"
                    };

                    return PostgreSQLConfiguration.PostgreSQL82.ConnectionString(builder.ToString());
                })
                .Mappings(m => m.FluentMappings
                    .Add<PhoneMap>()
                    .Add<AddressMap>()
                    .Add<UserMap>())
                
                .ExposeConfiguration(configuration =>
                {
                    configuration.SetProperty(Environment.Hbm2ddlKeyWords, "auto-quote");
                })
                .BuildSessionFactory())
                .As<ISessionFactory>()
                .SingleInstance();
            
            builder.Register(provider => provider.Resolve<ISessionFactory>().OpenSession())
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .As<IReadOnlyUserRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<UserAggregateStore>()
                .As<IUserAggregateStore>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AddressMapper>()
                .As<IMapper<Domain.Common.Address, Address>>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<PhoneMapper>()
                .As<IMapper<Domain.Common.Phone, Phone>>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<UserMapper>()
                .As<IMapper<Domain.Common.User, User>>()
                .InstancePerLifetimeScope();
            
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
