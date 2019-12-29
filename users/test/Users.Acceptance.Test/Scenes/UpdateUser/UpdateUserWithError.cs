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

namespace Users.Acceptance.Test.Scenes.UpdateUser
{
    [Story(
        IWant = "Try to update user info"
    )]
    public class UpdateUserWithError : BaseScene
    {
        private UpdateUserRequest _request;
        private UpdateUserReplay _replay;

        private ErrorResult _error;
        private string _firstName;
        private string _lastNames;
        private string _userId;

        public UpdateUserWithError()
        {
            _firstName = Fixture.Create<string>().Substring(0, 20);
            _lastNames = Fixture.Create<string>();
        }
        
        [Given(StepTitle = "Given new user")]
        private async Task GivenANewUser()
        {
            var addUserRequest = Fixture.Build<AddUserRequest>()
                .With(x => x.BirthDate, Timestamp.FromDateTime(Fixture.Create<DateTime>().AsUtc()))
                .With(x => x.FirstName, Fixture.Create<string>().Substring(0, 20))
                .With(x => x.Email, $"{Fixture.Create<string>()}@example.com")
                .Create();
            
            var replay = await Client.AddUsersAsync(addUserRequest);
            replay.IsSuccess.Should().BeTrue();
            _userId = replay.Value.Id;
        }

        [When(StepTitle = "When I Update User Info")]
        private async Task WhenIUpdateUserInfo()
        {
            _request = Fixture.Build<UpdateUserRequest>()
                .With(x => x.Id, _userId)
                .With(x => x.BirthDate, Timestamp.FromDateTime(Fixture.Create<DateTime>().AsUtc()))
                .With(x => x.FirstName, _firstName)
                .With(x => x.LastNames, _lastNames)
                .Create();
            
            _replay = await Client.UpdateUserAsync(_request);
        }

        [Then(StepTitle = "Then User info should be updated")]
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
    }
}