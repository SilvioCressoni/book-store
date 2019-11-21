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
using Users.Application.Mapper;
using Users.Application.Operations;
using Users.Domain;
using Xunit;

using static Xunit.Assert;
using Phone = Users.Application.Contracts.Response.Phone;

namespace Users.Application.Test
{
    public class PhoneGetOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly ILogger<PhoneGetOperation> _logger;
        private readonly IMapper<Domain.Common.Phone, Phone> _mapper;
        private readonly PhoneGetOperation _operation;
        private readonly Fixture _fixture;

        public PhoneGetOperationTest()
        {
            _fixture = new Fixture();
            
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _store = Substitute.For<IUserAggregateStore>();
            _logger = Substitute.For<ILogger<PhoneGetOperation>>();
            _mapper = Substitute.For<IMapper<Domain.Common.Phone, Phone>>();
            _operation = new PhoneGetOperation(_store, _logger, _mapper);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new PhoneGetOperation(null, _logger, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new PhoneGetOperation(_store, null, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_MapperIsNull()
            => Throws<ArgumentNullException>(() => new PhoneGetOperation(_store, _logger, null));
        
        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var phone = _fixture.Create<PhoneGet>();
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
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.Phone>());
        }


        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var phone = _fixture.Create<PhoneGet>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(phone.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            ISet<Domain.Common.Phone> phones = _fixture.Create<HashSet<Domain.Common.Phone>>();
            root.State
                .Returns(new UserState(new Domain.Common.User
                {
                    Phones = phones
                }));

            _mapper.Map(Arg.Any<Domain.Common.Phone>())
                .Returns(_fixture.Create<Phone>());
            
            var result = await _operation.ExecuteAsync(phone, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<IEnumerable<Phone>>>();
            ((OkResult<IEnumerable<Phone>>) result).Value.Should().HaveCount(phones.Count);
            
            await _store
                .Received(1)
                .GetAsync(phone.UserId, Arg.Any<CancellationToken>());

            _mapper
                .Received(phones.Count)
                .Map(Arg.Any<Domain.Common.Phone>());
        }

        [Fact]
        public async Task Execute_Should_ReturnError_When_ThrowException()
        {
            var phone = _fixture.Create<PhoneGet>();
            
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
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.Phone>());
        }
    }
}
