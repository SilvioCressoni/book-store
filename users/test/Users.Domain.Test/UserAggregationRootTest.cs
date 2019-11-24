using System;
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
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddPhone_Should_ReturnFail_When_NumberIsNull(string number)
        {
            var user = _fixture.Create<User>();
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddPhone(number);

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

        #region Address
        [Fact]
        public void AddAddress_Should_ReturnOk()
        {
            var user = _fixture.Create<User>();
            
            var line = string.Join(string.Empty, _fixture.CreateMany<char>(100));
            var postCode = string.Join(string.Empty, _fixture.CreateMany<char>(10));
            var number = Math.Abs(_fixture.Create<int>());
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddAddress(line, number, postCode);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(Result.Ok());
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddAddress_Should_ReturnFail_When_LineIsMissing(string line)
        {
            var user = _fixture.Create<User>();
            
            var postCode = string.Join(string.Empty, _fixture.CreateMany<char>(10));
            var number = Math.Abs(_fixture.Create<int>());
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddAddress(line, number, postCode);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.AddressError.MissingLine);
        }
        
        [Fact]
        public void AddAddress_Should_ReturnFail_When_LineIsOverThen100()
        {
            var user = _fixture.Create<User>();
            
            var line = string.Join(string.Empty, _fixture.CreateMany<char>(101));
            var postCode = string.Join(string.Empty, _fixture.CreateMany<char>(10));
            var number = Math.Abs(_fixture.Create<int>());
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddAddress(line, number, postCode);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.AddressError.InvalidLine);
        }
        
        [Fact]
        public void AddAddress_Should_ReturnFail_When_NumberIsBelowZero()
        {
            var user = _fixture.Create<User>();
            
            var line = string.Join(string.Empty, _fixture.CreateMany<char>(100));
            var postCode = string.Join(string.Empty, _fixture.CreateMany<char>(10));
            var number = Math.Abs(_fixture.Create<int>());
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddAddress(line, -number, postCode);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.AddressError.InvalidNumber);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddAddress_Should_ReturnFail_When_PostCodeIsMissing(string postCode)
        {
            var user = _fixture.Create<User>();
            
            var line = string.Join(string.Empty, _fixture.CreateMany<char>(100));
            var number = Math.Abs(_fixture.Create<int>());
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddAddress(line, number, postCode);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.AddressError.MissingPostCode);
        }
        
        [Fact]
        public void AddAddress_Should_ReturnFail_When_PostCodeIsOver10()
        {
            var user = _fixture.Create<User>();
            
            var line = string.Join(string.Empty, _fixture.CreateMany<char>(100));
            var postCode = string.Join(string.Empty, _fixture.CreateMany<char>(11));
            var number = Math.Abs(_fixture.Create<int>());
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddAddress(line, number, postCode);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.AddressError.InvalidPostCode);
        }
        
        [Fact]
        public void AddAddress_Should_ReturnFail_When_AddressExists()
        {
            var line = string.Join(string.Empty, _fixture.CreateMany<char>(100));
            var postCode = string.Join(string.Empty, _fixture.CreateMany<char>(10));
            var number = Math.Abs(_fixture.Create<int>());
         
            var user = _fixture.Build<User>()
                .With(x => x.Addresses, new List<Address>
                {
                    _fixture.Build<Address>()
                        .With(x => x.Line, line)
                        .With(x => x.Number, number)
                        .With(x => x.PostCode, postCode)
                        .Create()
                })
                .Create();
            
            var root = new UserAggregationRoot(new UserState(user));
            var result = root.AddAddress(line, number, postCode);
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.AddressError.AddressAlreadyExist);
        }

        #endregion

        #region User
        [Fact]
        public void Create_Should_ReturnOk()
        {
            var email = $"{_fixture.Create<string>()}@example.com";
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Create(email, firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(Result.Ok());
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_ReturnFail_When_EmailIsMissing(string email)
        {
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Create(email, firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.MissingEmail);
        }
        
        [Fact]
        public void Create_Should_ReturnFail_When_EmailIsInvalid()
        {
            var email = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Create(email, firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.InvalidEmail);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_ReturnFail_When_FirstNameIsMissing(string firstName)
        {
            var email = $"{_fixture.Create<string>()}@example.com";
            var lastName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Create(email, firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.MissingFirstName);
        }
        
        [Fact]
        public void Create_Should_ReturnFail_When_FirstNameIsInvalid()
        {
            var email = $"{_fixture.Create<string>()}@example.com";
            var firstName = string.Join(string.Empty, _fixture.CreateMany<char>(101));
            var lastName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Create(email, firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.InvalidFirstName);
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_ReturnFail_When_LastNamesIsMissing(string lastName)
        {
            var email = $"{_fixture.Create<string>()}@example.com";
            var firstName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Create(email, firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.MissingLastNames);
        }
        
        [Fact]
        public void Create_Should_ReturnFail_When_LastNamesIsInvalid()
        {
            var email = $"{_fixture.Create<string>()}@example.com";
            var firstName = _fixture.Create<string>();
            var lastName = string.Join(string.Empty, _fixture.CreateMany<char>(101));
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Create(email, firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.InvalidLastNames);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Update_Should_ReturnFail_When_FirstNameIsMissing(string firstName)
        {
            var lastName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Update(firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.MissingFirstName);
        }
        
        [Fact]
        public void Update_Should_ReturnFail_When_FirstNameIsInvalid()
        {
            var firstName = string.Join(string.Empty, _fixture.CreateMany<char>(101));
            var lastName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Update(firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.InvalidFirstName);
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Update_Should_ReturnFail_When_LastNamesIsMissing(string lastName)
        {
            var firstName = _fixture.Create<string>();
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Update(firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.MissingLastNames);
        }
        
        [Fact]
        public void Update_Should_ReturnFail_When_LastNamesIsInvalid()
        {
            var firstName = _fixture.Create<string>();
            var lastName = string.Join(string.Empty, _fixture.CreateMany<char>(101));
            var years = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
            
            var root = new UserAggregationRoot(new UserState(new User()));
            var result = root.Update(firstName, lastName, years);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(DomainError.UserError.InvalidLastNames);
        }
        #endregion
    }
}