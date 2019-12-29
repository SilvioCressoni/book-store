using System;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using Google.Protobuf.WellKnownTypes;
using TestStack.BDDfy;
using Users.Domain;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.AddUser
{
    [Story(IWant = "Try to create a new user with invalid data")]
    public class CreateNewUsersWithError : BaseScene
    {
        private AddUserRequest _userRequest;
        private AddUserReplay _replay;
        private string _email;
        private string _firstName;
        private string _lastNames;
        private readonly DateTime _birthDate;
        private ErrorResult _error;

        public CreateNewUsersWithError()
        {
            _email = $"{Fixture.Create<string>()}@example.com";
            _birthDate = new DateTime(1990, 1, 1).AsUtc();
            _firstName = Fixture.Create<string>().Substring(0, 20);
            _lastNames = Fixture.Create<string>();
        }
        
        [Given(StepTitle = "Given new user")]
        private void GivenANewUser()
        {
            _userRequest = new AddUserRequest
            {
                Email = _email,
                BirthDate = _birthDate.ToTimestamp(),
                FirstName = _firstName,
                LastNames = _lastNames
            };
        }

        [When(StepTitle = "When I Call API")]
        private async Task WhenICallAPI()
        {
            _replay = await Client.AddUsersAsync(_userRequest);
        }

        [Then(StepTitle = "Then I should get error")]
        private void ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(_error.ErrorCode);
            _replay.Description.Should().Be(_error.Description);
        }
        
        private string CreateText(int length)
        {
            var text = new StringBuilder();
            while (text.Length < length)
            {
                text.Append(Fixture.Create<string>());
            }

            return text.ToString().Substring(0, length);
        }
        
        [Theory]
        [InlineData(21)]
        public void InvalidFirstName(int length)
        {
            _firstName = CreateText(length);
            _error = DomainError.UserError.InvalidFirstName;
            this.BDDfy();
        }
        
        [Fact]
        public void MissingFirstName()
        {
            _firstName = string.Empty;
            _error = DomainError.UserError.MissingFirstName;
            this.BDDfy();
        }
        
        [Theory]
        [InlineData(101)]
        public void InvalidLastNames(int length)
        {
            _lastNames = CreateText(length);
            _error = DomainError.UserError.InvalidLastNames;
            this.BDDfy();
        }
        
        [Fact]
        public void MissingLastNames()
        {
            _lastNames = string.Empty;
            _error = DomainError.UserError.MissingLastNames;
            this.BDDfy();
        }
        
        [Theory]
        [InlineData(101)]
        public void InvalidEmailLength(int length)
        {
            _email = $"{CreateText(length)}@example.com";
            _error = DomainError.UserError.InvalidEmailLength;
            this.BDDfy();
        }
        
        [Fact]
        public void InvalidEmail()
        {
            _email = Fixture.Create<string>();
            _error = DomainError.UserError.InvalidEmail;
            this.BDDfy();
        }
        
        [Fact]
        public void MissingEmail()
        {
            _email = string.Empty;
            _error = DomainError.UserError.MissingEmail;
            this.BDDfy();
        }
    }
}