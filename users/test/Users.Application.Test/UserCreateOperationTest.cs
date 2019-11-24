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
    public class UserCreateOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly IReadOnlyUserRepository _repository;
        private readonly ILogger<UserCreateOperation> _logger;
        private readonly IMapper<Domain.Common.User, User> _mapper;
        private readonly UserCreateOperation _operation;
        private readonly Fixture _fixture;

        public UserCreateOperationTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _store = Substitute.For<IUserAggregateStore>();
            _logger = Substitute.For<ILogger<UserCreateOperation>>();
            _mapper = Substitute.For<IMapper<Domain.Common.User, User>>();
            _repository = Substitute.For<IUserRepository>();
            _operation = new UserCreateOperation(_store, _repository, _logger, _mapper);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new UserCreateOperation(null, _repository, _logger, _mapper));

        [Fact]
        public void Create_Should_Throw_When_RepositoryIsNull()
            => Throws<ArgumentNullException>(() => new UserCreateOperation(_store, null, _logger, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new UserCreateOperation(_store, _repository, null, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_MapperIsNull()
            => Throws<ArgumentNullException>(() => new UserCreateOperation(_store, _repository,_logger, null));

        [Fact]
        public async Task Execute_Should_ReturnError()
        {
            var request = _fixture.Create<UserAdd>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.Create()
                .Returns(root);

            var fail = _fixture.Create<ErrorResult>();

            root.Create(request.Email, request.FirstName, request.LastNames, request.BirthDay)
                .Returns(fail);

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().Be(fail);

            _store
                .Received(1)
                .Create();

            root
                .Received(1)
                .Create(request.Email, request.FirstName, request.LastNames, request.BirthDay);
            
            await _repository
                .DidNotReceive()
                .EmailExistAsync(request.Email, Arg.Any<CancellationToken>());
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.User>());
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task Execute_Should_ReturnError_When_EmailAlreadyExist()
        {
            var request = _fixture.Create<UserAdd>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.Create()
                .Returns(root);
            
            root.Create(request.Email, request.FirstName, request.LastNames, request.BirthDay)
                .Returns(Result.Ok());

            _repository.EmailExistAsync(request.Email, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(true));

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.EmailAlreadyExist);

            _store
                .Received(1)
                .Create();

            root
                .Received(1)
                .Create(request.Email, request.FirstName, request.LastNames, request.BirthDay);

            await _repository
                .Received(1)
                .EmailExistAsync(request.Email, Arg.Any<CancellationToken>());
            
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
            var request = _fixture.Create<UserAdd>();
            var root = Substitute.For<IUserAggregationRoot>();
            root.State.Returns(new UserState(_fixture.Create<Domain.Common.User>()));

            _store.Create()
                .Returns(root);

            root.Create(request.Email, request.FirstName, request.LastNames, request.BirthDay)
                .Returns(Result.Ok());
            
            _repository.EmailExistAsync(request.Email, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(false));

            _mapper.Map(Arg.Any<Domain.Common.User>())
                .Returns(_fixture.Create<User>());

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<User>>();
            
            _store
                .Received(1)
                .Create();

            root
                .Received(1)
                .Create(request.Email, request.FirstName, request.LastNames, request.BirthDay);

            await _repository
                .Received(1)
                .EmailExistAsync(request.Email, Arg.Any<CancellationToken>());
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
            var request = _fixture.Create<UserAdd>();
            var root = Substitute.For<IUserAggregationRoot>();
            root.State.Returns(new UserState(_fixture.Create<Domain.Common.User>()));

            _store.Create()
                .Returns(root);
            
            root.Create(request.Email, request.FirstName, request.LastNames, request.BirthDay)
                .Returns(Result.Ok());
            
            var exception = _fixture.Create<Exception>();
            _repository.EmailExistAsync(request.Email, Arg.Any<CancellationToken>())
                .Throws(exception);

            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Should().BeOfType<ErrorResult>();
            result.Should().BeEquivalentTo(Result.Fail(exception));

            _store
                .Received(1)
                .Create();

            root
                .Received(1)
                .Create(request.Email, request.FirstName, request.LastNames, request.BirthDay);

            await _repository
                .Received(1)
                .EmailExistAsync(request.Email, Arg.Any<CancellationToken>());

            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.User>());

            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }
    }
}
