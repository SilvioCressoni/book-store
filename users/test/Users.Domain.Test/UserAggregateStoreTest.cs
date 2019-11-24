using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Domain.Common;
using Users.Infrastructure;
using Xunit;

using static Xunit.Assert;
namespace Users.Domain.Test
{
    public class UserAggregateStoreTest
    {
        private readonly Fixture _fixture;
        private readonly IUserRepository _repository;
        private readonly UserAggregateStore _store;

        public UserAggregateStoreTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repository = Substitute.For<IUserRepository>();
            _store = new UserAggregateStore(_repository);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new UserAggregateStore(null));
        
        [Fact]
        public void Create_Should_ReturnOk()
        {
            var root = _store.Create();
            root.Should().NotBeNull();
            root.State.Should().NotBeNull();
            ((Common.User)root.State).Should().BeEquivalentTo(new Common.User());
        }
        
        [Fact]
        public void SaveAsync_Should_ReturnOk_When_SaveIsSuccess()
        {
            var root = _store.Create();
            _repository.SaveAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);
            _store.SaveAsync(root);
        }
        
        [Fact]
        public async Task SaveAsync_Should_Fail_When_SaveThrow()
        {
            var root = _store.Create();
            var exception = _fixture.Create<Exception>();
            _repository.SaveAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
                .Throws(exception);
            
            var e = await ThrowsAsync<Exception>(async () => await _store.SaveAsync(root));
            e.Should().BeEquivalentTo(exception);
        }
        
        [Fact]
        public async Task GetById_Should_Ok_When_RepositoryReturnOk()
        {
            var id = _fixture.Create<Guid>();
            var user = _fixture.Create<Common.User>();

            _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(user));

            var root = await _store.GetAsync(id);

            root.Should().NotBeNull();
            root.State.Should().NotBeNull();
            ((Common.User)root.State).Should().BeEquivalentTo(user);
            
            await _repository
                .Received(1)
                .GetByIdAsync(id, Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task GetById_Should_ReturnNull_When_RepositoryReturnNull()
        {
            var id = _fixture.Create<Guid>();

            _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User>(null));

            var root = await _store.GetAsync(id);

            root.Should().BeNull();
            
            await _repository
                .Received(1)
                .GetByIdAsync(id, Arg.Any<CancellationToken>());
        }
    }
}
