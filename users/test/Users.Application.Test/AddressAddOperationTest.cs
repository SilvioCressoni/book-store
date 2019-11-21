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
    public class AddressAddOperationTest
    {
        private readonly IUserAggregateStore _store;
        private readonly ILogger<AddressAddOperation> _logger;
        private readonly IMapper<Domain.Common.Address, Address> _mapper;
        private readonly AddressAddOperation _operation;
        private readonly Fixture _fixture;

        public AddressAddOperationTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _store = Substitute.For<IUserAggregateStore>();
            _logger = Substitute.For<ILogger<AddressAddOperation>>();
            _mapper = Substitute.For<IMapper<Domain.Common.Address, Address>>();
            _operation = new AddressAddOperation(_store, _logger, _mapper);
        }

        [Fact]
        public void Create_Should_Throw_When_StoreIsNull()
            => Throws<ArgumentNullException>(() => new AddressAddOperation(null, _logger, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_LoggerIsNull()
            => Throws<ArgumentNullException>(() => new AddressAddOperation(_store, null, _mapper));
        
        [Fact]
        public void Create_Should_Throw_When_MapperIsNull()
            => Throws<ArgumentNullException>(() => new AddressAddOperation(_store, _logger, null));
        
        [Fact]
        public async Task Execute_Should_ReturnUserNotFound()
        {
            var address = _fixture.Create<AddressAdd>();
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
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Execute_Should_ReturnError()
        {
            var address = _fixture.Create<AddressAdd>();
            var root = Substitute.For<IUserAggregationRoot>();

            _store.GetAsync(address.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            var fail = _fixture.Create<ErrorResult>();

            root.AddAddress(address.Line, address.Number, address.PostCode)
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
                .AddAddress(address.Line, address.Number, address.PostCode);
            
            _mapper
                .DidNotReceive()
                .Map(Arg.Any<Domain.Common.Address>());
            
            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Execute_Should_ReturnOk()
        {
            var address = _fixture.Create<AddressAdd>();
            var root = Substitute.For<IUserAggregationRoot>();
            root.State.Returns(new UserState(new Domain.Common.User
            {
                Addresses = _fixture.Create<List<Domain.Common.Address>>()
            }));

            _store.GetAsync(address.UserId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(root));

            root.AddAddress(address.Line, address.Number, address.PostCode)
                .Returns(Result.Ok());

            _mapper.Map(Arg.Any<Domain.Common.Address>())
                .Returns(new Address()
                {
                    Id = _fixture.Create<Guid>(),
                    Line = address.Line,
                    Number = address.Number,
                    PostCode = address.PostCode
                });

            var result = await _operation.ExecuteAsync(address, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Should().BeOfType<OkResult<Address>>();
            ((OkResult<Address>) result).Value.Number.Should().Be(address.Number);
            ((OkResult<Address>) result).Value.Line.Should().Be(address.Line);
            ((OkResult<Address>) result).Value.PostCode.Should().Be(address.PostCode);
            
            await _store
                .Received(1)
                .GetAsync(address.UserId, Arg.Any<CancellationToken>());

            root
                .Received(1)
                .AddAddress(address.Line, address.Number, address.PostCode);
            
            _mapper
                .Received(1)
                .Map(Arg.Any<Domain.Common.Address>());
            
            await _store
                .Received(1)
                .SaveAsync(root, Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task Execute_Should_ReturnError_When_ThrowException()
        {
            var address = _fixture.Create<AddressAdd>();
            
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

            await _store
                .DidNotReceive()
                .SaveAsync(Arg.Any<IUserAggregationRoot>(), Arg.Any<CancellationToken>());
        }
    }
}
