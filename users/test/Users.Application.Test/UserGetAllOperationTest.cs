using System;
using System.Collections.Generic;
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
    public class UserGetAllOperationTest
    {
        private readonly IReadOnlyUserRepository _repository;
        private readonly ILogger<UserGetAllOperation> _logger;
        private readonly IMapper<Domain.Common.User, User> _mapper;
        private readonly UserGetAllOperation _operation;
        private readonly Fixture _fixture;

        public UserGetAllOperationTest()
        {
            _fixture = new Fixture();
            
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _repository = Substitute.For<IReadOnlyUserRepository>();
            _logger = Substitute.For<ILogger<UserGetAllOperation>>();
            _mapper = Substitute.For<IMapper<Domain.Common.User, User>>();
            _operation = new UserGetAllOperation(_repository, _logger, _mapper);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new UserGetAllOperation(null, _logger, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new UserGetAllOperation(_repository, null, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_MapperIsNull()
            => Throws<ArgumentNullException>(() => new UserGetAllOperation(_repository, _logger, null));

        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var request = _fixture.Create<UserGetAll>();

            var users = _fixture.Create<List<Domain.Common.User>>();
            _repository.GetAll(request.Skip, request.Take)
                .Returns(users);

            _mapper.Map(Arg.Any<Domain.Common.User>())
                .Returns(_fixture.Create<User>());
            
            var result = await _operation.ExecuteAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<IEnumerable<User>>>();
            ((OkResult<IEnumerable<User>>) result).Value.Should().HaveCount(users.Count);

            _repository
                .Received(1)
                .GetAll(request.Skip, request.Take);

            _mapper
                .Received(users.Count)
                .Map(Arg.Any<Domain.Common.User>());
        }

        //[Fact]
        //public async Task Execute_Should_ReturnError_When_ThrowException()
        //{
        //    var exception = _fixture.Create<Exception>();
        //    var request = _fixture.Create<UserGetAll>();

        //    _repository.GetAll(request.Skip, request.Take)
        //        .Throws(exception);

        //    var result = await _operation.ExecuteAsync(request, CancellationToken.None);

        //    result.IsSuccess.Should().BeFalse();
        //    result.Value.Should().BeNull();
        //    result.Should().BeOfType<ErrorResult>();
        //    result.ErrorCode.Should().Be(exception.HResult.ToString());
        //    result.Description.Should().Be(exception.ToString());

        //     _repository
        //        .Received(1)
        //        .GetAll(request.Skip, request.Take);

        //    _mapper
        //        .DidNotReceive()
        //        .Map(Arg.Any<Domain.Common.User>());
        //}
    }
}
