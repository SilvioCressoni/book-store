using System;
using System.Data.SqlClient;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using Npgsql;
using Users.Application.Contracts.Response;
using Users.Application.Mapper;
using Users.Application.Operations;
using Users.Domain;
using Users.Infrastructure.Extensions;
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
            services.AddSwaggerDocument();
            //services.AddGrpc();

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
            });

            services.AddSingleton(provider => Fluently.Configure()
                .Database(() =>
                {
                    var configure = provider.GetRequiredService<IConfiguration>();
                    var connection = configure.GetConnectionString("Postgres");
                    
                    var builder = new NpgsqlConnectionStringBuilder(connection)
                    {
                        ApplicationName = "BookStoreUser"
                    };

                    return PostgreSQLConfiguration.Standard.ConnectionString(builder.ToString());
                })
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory());

            services.AddScoped(provider => provider.GetRequiredService<ISessionFactory>().OpenSession());
            services.AddScoped<IUserAggregateStore, UserAggregateStore>();

            services.AddSingleton<IMapper<Domain.Common.Address, Address>, AddressMapper>();
            services.AddSingleton<IMapper<Domain.Common.Phone, Phone>, PhoneMapper>();
            services.AddSingleton<IMapper<Domain.Common.User, User>, UserMapper>();
            
            services.AddScoped<PhoneAddOperation>();
            services.AddScoped<PhoneRemoveOperation>();
            services.AddScoped<PhoneGetOperation>();

            services.AddScoped<AddressAddOperation>();
            services.AddScoped<AddressRemoveOperation>();
            services.AddScoped<AddressGetOperation>();

            services.AddScoped<UserCreateOperation>();
            services.AddScoped<UserUpdateOperation>();
            services.AddScoped<UserGetOperation>();
            services.AddScoped<UserGetAllOperation>();
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
