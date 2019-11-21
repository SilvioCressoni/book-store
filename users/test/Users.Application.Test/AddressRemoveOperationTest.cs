using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Application.Contracts;
using Users.Application.Operations;
using Users.Domain;
using Xunit;

using static Xunit.Assert;
namespace Users.Application.Test
{
    public class AddressRemoveOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly ILogger<AddressRemoveOperation> _logger;
        private readonly AddressRemoveOperation _operation;
        private readonly Fixture _fixture;

        public AddressRemoveOperationTest()
        {
            _fixture = new Fixture();
            _store = Substitute.For<IUserAggregateStore>();
            _logger = Substitute.For<ILogger<AddressRemoveOperation>>();
            _operation = new AddressRemoveOperation(_store, _logger);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new AddressRemoveOperation(null, _logger));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new AddressRemoveOperation(_store,null));

        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var address = _fixture.Create<AddressRemove>();
            _store.GetAsync(address.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((IUserAggregationRoot) null));

            var result = await _operation.ExecuteAsync(address, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.ErrorCode.Should().NotBeNullOrEmpty();
            result.Description.Should().NotBeNullOrEmpty();
            result.Should().Be(DomainError.UserError.UserNotFound);

            await _store
                .Received(1)
                .GetAsync(address.UserId, Arg.Any<CancellationToken>());

            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Execute_Should_ReturnError()
        {
            var address = _fixture.Create<AddressRemove>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(address.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            var fail = _fixture.Create<ErrorResult>();

            root.RemoveAddress(address.Id)
                .Returns(fail);

            var result = await _operation.ExecuteAsync(address, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().Be(fail);
            
            await _store
                .Received(1)
                .GetAsync(address.UserId, Arg.Any<CancellationToken>());

            root
                .Received(1)
                .RemoveAddress(address.Id);

                await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var address = _fixture.Create<AddressRemove>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(address.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            root.RemoveAddress(address.Id)
                .Returns(Result.Ok());

            var result = await _operation.ExecuteAsync(address, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
            result.Should().BeOfType<OkResult>();
            
            await _store
                .Received(1)
                .GetAsync(address.UserId, Arg.Any<CancellationToken>());

            root
                .Received(1)
                .RemoveAddress(address.Id);
            
            await _store
                .Received(1)
                .SaveAsync(root, Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task Execute_Should_ReturnError_When_ThrowException()
        {
            var address = _fixture.Create<AddressRemove>();
            
            var exception = _fixture.Create<Exception>();
            _store.GetAsync(address.UserId, Arg.Any<CancellationToken>())
                .Throws(exception);

            var result = await _operation.ExecuteAsync(address, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().BeOfType<ErrorResult>();
            result.ErrorCode.Should().Be(exception.HResult.ToString());
            result.Description.Should().Be(exception.ToString());

            await _store
                .Received(1)
                .GetAsync(address.UserId, Arg.Any<CancellationToken>());
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }
    }
}
