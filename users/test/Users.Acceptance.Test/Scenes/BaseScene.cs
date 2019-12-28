using System;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using TestStack.BDDfy;
using Xunit;

namespace Users.Acceptance.Test.Scenes
{
    public abstract class BaseScene
    {
        protected Fixture Fixture { get; }
        protected IServiceProvider Provider { get; }
        protected Users.Web.Proto.Users.UsersClient Client { get; }
        
        protected BaseScene()
        {
            Fixture = new Fixture();
            Provider = DI.Provider.CreateScope().ServiceProvider;
            Client = Provider.GetRequiredService<Users.Web.Proto.Users.UsersClient>();
        }
        
        [Fact]
        public virtual void Execute()
        {
            this.BDDfy();
        }
    }
}