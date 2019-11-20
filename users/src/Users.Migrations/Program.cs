using System;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Users.Migrations
{
    public static class Program
    {
        public static Task Main(string[] args)
        {

        }


        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(AddPhoneTable).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static async Task EnsureDatabaseAsync(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var database = builder.Database;
            builder.Database = null;

            await using var connection = new NpgsqlConnection(builder.ToString());
            connection.Open();
            var databasesQuery = "select * from postgres.pg_catalog.pg_database where datname = @name";

            await using (var command = new NpgsqlCommand(databasesQuery, connection))
            {
                await using var result = command.ExecuteReader();
                if (result.HasRows)
                {
                    return;
                }
            }

            var createDatabaseQuery = $"CREATE DATABASE \"{database}\"";
            await using var create = new NpgsqlCommand(databasesQuery, connection);
            await create.ExecuteNonQueryAsync();
        }   
    }
}