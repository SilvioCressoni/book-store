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
            services.AddGrpc();

            services.AddSingleton(provider => Fluently.Configure()
                .Database(() =>
                {
                    var configure = provider.GetRequiredService<IConfiguration>();
                    var connection = configure.GetConnectionString("Postgres");

                    if (connection.IsMissing())
                    {
                        var builder = new NpgsqlConnectionStringBuilder(connection)
                        {
                            ApplicationName = "BookStoreUser"
                        };

                        return PostgreSQLConfiguration.Standard.ConnectionString(builder.ToString());
                    }

                    connection = configure.GetConnectionString("SqlServer");
                    if (connection.IsMissing())
                    {
                        var builder = new SqlConnectionStringBuilder(connection)
                        {
                            ApplicationName = "BookStoreUser"
                        };

                        return MsSqlConfiguration.MsSql2012.ConnectionString(builder.ToString());
                    }

                    throw new NotSupportedException();
                })
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory());

            services.AddScoped(provider => provider.GetRequiredService<ISessionFactory>().OpenSession());
            services.AddScoped<IUserAggregateStore, UserAggregateStore>();
            
            services.AddScoped<PhoneAddOperation>();
            services.AddScoped<PhoneRemoveOperation>();

            services.AddScoped<AddressAddOperation>();
            services.AddScoped<AddressRemoveOperation>();

            services.AddScoped<UserCreateOperation>();
            services.AddScoped<UserUpdateOperation>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
