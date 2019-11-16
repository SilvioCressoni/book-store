using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Users.Application.Contracts;
using Users.Application.Operations;
using Users.Domain;
using Xunit;

using static Xunit.Assert;
namespace Users.Application.Test
{
    public class PhoneRemoveOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly PhoneRemoveOperation _operation;
        private readonly Fixture _fixture;

        public PhoneRemoveOperationTest()
        {
            _fixture = new Fixture();
            _store = Substitute.For<IUserAggregateStore>();
            _operation = new PhoneRemoveOperation(_store);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new PhoneRemoveOperation(null));

        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var phone = _fixture.Create<Phone>();
            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((IUserAggregationRoot) null));

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Error.Should().NotBeNullOrEmpty();
            result.Description.Should().NotBeNullOrEmpty();
            result.Should().Be(DomainError.UserError.UserNotFound);
        }


        [Fact]
        public async Task Execute_Should_ReturnError()
        {
            var phone = _fixture.Create<Phone>();
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
        }

        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var phone = _fixture.Create<Phone>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            root.RemovePhone(phone.Number)
                .Returns(Result.Ok());

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
            result.Should().BeOfType<OkResult>();
            
        }
    }
}
