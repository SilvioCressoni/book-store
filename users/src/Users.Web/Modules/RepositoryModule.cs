using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using Npgsql;
using Users.Infrastructure;
using Users.Infrastructure.Mapper;

namespace Users.Web.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(provider => Fluently.Configure(new Configuration()
                        .SetNamingStrategy(new PostgresNamingStrategy()))
                    .Database(() =>
                    {
                        var configure = provider.Resolve<IConfiguration>();
                        var connection = configure.GetConnectionString("Postgres");

                        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connection)
                        {
                            ApplicationName = "BookStoreUser"
                        };

                        return PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionStringBuilder.ToString());
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
        }
    }
}
