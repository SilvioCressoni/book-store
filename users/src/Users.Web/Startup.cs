using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using Users.Web.Modules;
using Users.Web.Services;

namespace Users.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvcCore()
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.IgnoreNullValues = true;
                });
            
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

            services.AddGrpc();
            services.AddMemoryCache(opt => opt.ExpirationScanFrequency = TimeSpan.FromMinutes(5));
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
            builder.RegisterModule<AggregationModule>()
                .RegisterModule<MapperModule>()
                .RegisterModule<OperationModule>()
                .RegisterModule<RepositoryModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserService>();
                endpoints.MapControllers();
            });
        }
    }
}
