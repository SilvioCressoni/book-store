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
using Users.Infrastructure;
using Xunit;

using static Xunit.Assert;

namespace Users.Application.Test
{
    public class UserGetOperationTest
    {
        private readonly IReadOnlyUserRepository _repository;
        private readonly ILogger<UserGetOperation> _logger;
        private readonly IMapper<Domain.Common.User, User> _mapper;
        private readonly UserGetOperation _operation;
        private readonly Fixture _fixture;

        public UserGetOperationTest()
        {
            _fixture = new Fixture();
            
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _repository = Substitute.For<IReadOnlyUserRepository>();
            _logger = Substitute.For<ILogger<UserGetOperation>>();
            _mapper = Substitute.For<IMapper<Domain.Common.User, User>>();
            _operation = new UserGetOperation(_repository, _logger, _mapper);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new UserGetOperation(null, _logger, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new UserGetOperation(_repository, null, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_MapperIsNull()
            => Throws<ArgumentNullException>(() => new UserGetOperation(_repository, _logger, null));
        
        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var request = _fixture.Create<UserGet>();
            _repository.GetByIdAsync(request.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult((Domain.Common.User) null));

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.ErrorCode.Should().NotBeNullOrEmpty();
            result.Description.Should().NotBeNullOrEmpty();
            result.Should().Be(DomainError.UserError.UserNotFound);

            await _repository
                .Received(1)
                .GetByIdAsync(request.Id, Arg.Any<CancellationToken>());
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.User>());
        }


        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var request = _fixture.Create<UserGet>();
            var user = _fixture.Create<Domain.Common.User>();

            _repository.GetByIdAsync(request.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(user));

            _mapper.Map(Arg.Any<Domain.Common.User>())
                .Returns(_fixture.Create<User>());
            
            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<User>>();

            await _repository
                .Received(1)
                .GetByIdAsync(request.Id, Arg.Any<CancellationToken>());

            _mapper
                .Received(1)
                .Map(Arg.Any<Domain.Common.User>());
        }

        [Fact]
        public async Task Execute_Should_ReturnError_When_ThrowException()
        {
            var request = _fixture.Create<UserGet>();
            var exception = _fixture.Create<Exception>();

            _repository.GetByIdAsync(request.Id, Arg.Any<CancellationToken>())
                .Throws(exception);

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().BeOfType<ErrorResult>();
            result.ErrorCode.Should().Be(exception.HResult.ToString());
            result.Description.Should().Be(exception.ToString());

             await _repository
                .Received(1)
                .GetByIdAsync(request.Id, Arg.Any<CancellationToken>());
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.User>());
        }
    }
}
