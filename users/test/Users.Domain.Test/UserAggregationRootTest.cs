using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Users.Domain.Common;
using Xunit;

namespace Users.Domain.Test
{
    public class UserAggregationRootTest
    {
        private readonly Fixture _fixture;

        public UserAggregationRootTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        #region Phone
        [Fact]
        public void AddPhone_Should_ReturnOk()
        {
            var user = _fixture.Create<User>();
            var number = string.Join(string.Empty, _fixture.CreateMany<char>(14));
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddPhone(number);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(Result.Ok());
        }
        
        [Fact]
        public void AddPhone_Should_ReturnFail_When_NumberIsNull()
        {
            var user = _fixture.Create<User>();
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddPhone(null);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.PhoneError.MissingNumber);
        }
        
        [Fact]
        public void AddPhone_Should_ReturnFail_When_NumberIsInvalid()
        {
            var user = _fixture.Create<User>();
            var number = string.Join(string.Empty, _fixture.CreateMany<char>(16));
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddPhone(number);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.PhoneError.InvalidNumber);
        }
        
        [Fact]
        public void AddPhone_Should_ReturnFail_When_NumberAlreadyAdded()
        {
            var number = string.Join(string.Empty, _fixture.CreateMany<char>(14));
            var user = _fixture.Build<User>()
                .With(x => x.Phones, new HashSet<Phone>
                {
                    new Phone
                    {
                        Number = number
                    }
                })
                .Create();
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddPhone(number);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.PhoneError.NumberAlreadyExist);
        }
        
        [Fact]
        public void RemovePhone_Should_ReturnOk()
        {
            var number = _fixture.Create<string>();
            var user = _fixture.Build<User>()
                .With(x => x.Phones, new HashSet<Phone>
                {
                    new Phone
                    {
                        Number =  number
                    }
                })
                .Create();
            
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.RemovePhone(number);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(Result.Ok());
        }

        [Fact]
        public void RemovePhone_Should_ReturnFail_When_NumberIsMissing()
        {
            var user = _fixture.Create<User>();

            var root = new UserAggregationRoot(new UserState(user));
            var result = root.RemovePhone(null);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.PhoneError.MissingNumber);
        }
        
        [Fact]
        public void RemovePhone_Should_ReturnFail_When_NumberNotFound()
        {
            var number = _fixture.Create<string>();
            var user = _fixture.Create<User>();

            var root = new UserAggregationRoot(new UserState(user));
            var result = root.RemovePhone(number);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.PhoneError.NumberNotFound);
        }
        #endregion
    }
}