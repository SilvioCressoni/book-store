using System;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Acceptance.Test.Scenes
{
    public abstract class BaseScene
    {
        protected Fixture Fixture { get; }
        protected IServiceProvider Provider { get; }
        
        protected Web.Proto.Users.UsersClient Client { get; }
        
        protected BaseScene()
        {
            Fixture = new Fixture();
            Provider = DI.Provider.CreateScope().ServiceProvider;
            Client = Provider.GetRequiredService<Web.Proto.Users.UsersClient>();
        }
    }
}