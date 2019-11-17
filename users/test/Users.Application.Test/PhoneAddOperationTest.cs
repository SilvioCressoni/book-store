using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Users.Application.Contracts.Request;
using Users.Application.Operations;
using Users.Domain;
using Xunit;

using static Xunit.Assert;
namespace Users.Application.Test
{
    public class PhoneAddOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly PhoneAddOperation _operation;
        private readonly Fixture _fixture;

        public PhoneAddOperationTest()
        {
            _fixture = new Fixture();
            _store = Substitute.For<IUserAggregateStore>();
            _operation = new PhoneAddOperation(_store);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new PhoneAddOperation(null));


        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var phone = _fixture.Create<PhoneAdd>();
            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((IUserAggregationRoot) null));

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.ErrorCode.Should().NotBeNullOrEmpty();
            result.Description.Should().NotBeNullOrEmpty();
            result.Should().Be(DomainError.UserError.UserNotFound);
        }


        [Fact]
        public async Task Execute_Should_ReturnError()
        {
            var phone = _fixture.Create<PhoneAdd>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            var fail = _fixture.Create<ErrorResult>();

            root.AddPhone(phone.Number)
                .Returns(fail);

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().Be(fail);
        }

        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var phone = _fixture.Create<PhoneAdd>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            root.AddPhone(phone.Number)
                .Returns(Result.Ok());

            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<PhoneAdd>>();
            ((OkResult<PhoneAdd>) result).Value.Should().Be(phone);
        }
    }
}
