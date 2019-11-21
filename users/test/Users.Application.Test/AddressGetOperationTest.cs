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
using Xunit;

using static Xunit.Assert;

namespace Users.Application.Test
{
    public class AddressGetOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly ILogger<AddressGetOperation> _logger;
        private readonly IMapper<Domain.Common.Address, Address> _mapper;
        private readonly AddressGetOperation _operation;
        private readonly Fixture _fixture;

        public AddressGetOperationTest()
        {
            _fixture = new Fixture();
            
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _store = Substitute.For<IUserAggregateStore>();
            _logger = Substitute.For<ILogger<AddressGetOperation>>();
            _mapper = Substitute.For<IMapper<Domain.Common.Address, Address>>();
            _operation = new AddressGetOperation(_store, _logger, _mapper);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new AddressGetOperation(null, _logger, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new AddressGetOperation(_store, null, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_MapperIsNull()
            => Throws<ArgumentNullException>(() => new AddressGetOperation(_store, _logger, null));
        
        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var address = _fixture.Create<AddressGet>();
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
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.Address>());
        }


        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var address = _fixture.Create<AddressGet>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(address.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            ICollection<Domain.Common.Address> addresses = _fixture.Create<List<Domain.Common.Address>>();
            root.State
                .Returns(new UserState(new Domain.Common.User
                {
                    Addresses = addresses
                }));

            _mapper.Map(Arg.Any<Domain.Common.Address>())
                .Returns(_fixture.Create<Address>());
            
            var result = await _operation.ExecuteAsync(address, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<IEnumerable<Address>>>();
            ((OkResult<IEnumerable<Address>>) result).Value.Should().HaveCount(addresses.Count);
            
            await _store
                .Received(1)
                .GetAsync(address.UserId, Arg.Any<CancellationToken>());

            _mapper
                .Received(addresses.Count)
                .Map(Arg.Any<Domain.Common.Address>());
        }

        [Fact]
        public async Task Execute_Should_ReturnError_When_ThrowException()
        {
            var address = _fixture.Create<AddressGet>();
            
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
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.Address>());
        }
    }
}
