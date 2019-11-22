using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Application.Mapper;
using Users.Application.Operations;
using Users.Domain;
using Xunit;

using static Xunit.Assert;

namespace Users.Application.Test
{
    public class UserUpdateOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly ILogger<UserUpdateOperation> _logger;
        private readonly IMapper<Domain.Common.User, User> _mapper;
        private readonly UserUpdateOperation _operation;
        private readonly Fixture _fixture;

        public UserUpdateOperationTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _store = Substitute.For<IUserAggregateStore>();
            _logger = Substitute.For<ILogger<UserUpdateOperation>>();
            _mapper = Substitute.For<IMapper<Domain.Common.User, User>>();
            _operation = new UserUpdateOperation(_store, _logger, _mapper);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new UserUpdateOperation(null, _logger, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new UserUpdateOperation(_store, null, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_MapperIsNull()
            => Throws<ArgumentNullException>(() => new UserUpdateOperation(_store, _logger, null));
        
        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var request = _fixture.Create<UserUpdate>();
            _store.GetAsync(request.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((IUserAggregationRoot) null));

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.ErrorCode.Should().NotBeNullOrEmpty();
            result.Description.Should().NotBeNullOrEmpty();
            result.Should().Be(DomainError.UserError.UserNotFound);

            await _store
                .Received(1)
                .GetAsync(request.Id, Arg.Any<CancellationToken>());

            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.User>());
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Execute_Should_ReturnError()
        {
            var request = _fixture.Create<UserUpdate>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(request.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            var fail = _fixture.Create<ErrorResult>();

            root.Update(request.FirstName, request.LastNames, request.BirthDay)
                .Returns(fail);

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().Be(fail);
            
            await _store
                .Received(1)
                .GetAsync(request.Id, Arg.Any<CancellationToken>());

            root
                .Received(1)
                .Update(request.FirstName, request.LastNames, request.BirthDay);
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.User>());
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var request = _fixture.Create<UserUpdate>();
            var root = Substitute.For<IUserAggregationRoot>();
            root.State.Returns(new UserState(_fixture.Create<Domain.Common.User>()));

            _store.GetAsync(request.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            root.Update(request.FirstName, request.LastNames, request.BirthDay)
                .Returns(Result.Ok());

            _mapper.Map(Arg.Any<Domain.Common.User>())
                .Returns(_fixture.Create<User>());

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<User>>();
            
            await _store
                .Received(1)
                .GetAsync(request.Id, Arg.Any<CancellationToken>());

            root
                .Received(1)
                .Update(request.FirstName, request.LastNames, request.BirthDay);
            
            _mapper
                .Received(1)
                .Map(Arg.Any<Domain.Common.User>());
            
            await _store
                .Received(1)
                .SaveAsync(root, Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task Execute_Should_ReturnError_When_ThrowException()
        {
            var request = _fixture.Create<UserUpdate>();
            
            var exception = _fixture.Create<Exception>();
            _store.GetAsync(request.Id, Arg.Any<CancellationToken>())
                .Throws(exception);

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().BeOfType<ErrorResult>();
            result.ErrorCode.Should().Be(exception.HResult.ToString());
            result.Description.Should().Be(exception.ToString());

            await _store
                .Received(1)
                .GetAsync(request.Id, Arg.Any<CancellationToken>());

            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.User>());

            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }
    }
}
