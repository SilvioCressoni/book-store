using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Application.Contracts.Request;
using Users.Application.Operations;
using Users.Domain;
using Xunit;

using static Xunit.Assert;
namespace Users.Application.Test
{
    public class PhoneRemoveOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly ILogger<PhoneRemoveOperation> _logger;
        private readonly PhoneRemoveOperation _operation;
        private readonly Fixture _fixture;

        public PhoneRemoveOperationTest()
        {
            _fixture = new Fixture();
            _store = Substitute.For<IUserAggregateStore>();
            _logger = Substitute.For<ILogger<PhoneRemoveOperation>>();
            _operation = new PhoneRemoveOperation(_store, _logger);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new PhoneRemoveOperation(null, _logger));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new PhoneRemoveOperation(_store,null));

        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var phone = _fixture.Create<PhoneRemove>();
            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((IUserAggregationRoot) null));

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.ErrorCode.Should().NotBeNullOrEmpty();
            result.Description.Should().NotBeNullOrEmpty();
            result.Should().Be(DomainError.UserError.UserNotFound);

            await _store
                .Received(1)
                .GetAsync(phone.UserId, Arg.Any<CancellationToken>());

            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Execute_Should_ReturnError()
        {
            var phone = _fixture.Create<PhoneRemove>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            var fail = _fixture.Create<ErrorResult>();

            root.RemovePhone(phone.Number)
                .Returns(fail);

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().Be(fail);
            
            await _store
                .Received(1)
                .GetAsync(phone.UserId, Arg.Any<CancellationToken>());

            root
                .Received(1)
                .RemovePhone(phone.Number);
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var phone = _fixture.Create<PhoneRemove>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            root.RemovePhone(phone.Number)
                .Returns(Result.Ok());

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
            result.Should().BeOfType<OkResult>();
            
            await _store
                .Received(1)
                .GetAsync(phone.UserId, Arg.Any<CancellationToken>());

            root
                .Received(1)
                .RemovePhone(phone.Number);
            
            await _store
                .Received(1)
                .SaveAsync(root, Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task Execute_Should_ReturnError_When_ThrowException()
        {
            var phone = _fixture.Create<PhoneRemove>();
            
            var exception = _fixture.Create<Exception>();
            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Throws(exception);

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().BeOfType<ErrorResult>();
            result.ErrorCode.Should().Be(exception.HResult.ToString());
            result.Description.Should().Be(exception.ToString());

            await _store
                .Received(1)
                .GetAsync(phone.UserId, Arg.Any<CancellationToken>());
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }
    }
}
