using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using Google.Protobuf.WellKnownTypes;
using TestStack.BDDfy;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.AddUser
{
    [Story(
        IWant = "Create a new user",
        SoThat = "I can use system"
    )]
    public class CreateNewUserWithSuccess : BaseScene
    {
        private AddUserRequest _userRequest;
        private AddUserReplay _replay;
        [Given(StepTitle = "Given new user")]
        private void GivenANewUser()
        {
            _userRequest = new AddUserRequest
            {
                Email = $"{Fixture.Create<string>()}@example.com",
                BirthDate = new DateTime(1990, 1, 1).AsUtc().ToTimestamp(),
                FirstName = Fixture.Create<string>().Substring(0, 20),
                LastNames = Fixture.Create<string>()
            };
        }

        [When(StepTitle = "When I Call API")]
        private async Task WhenICallAPI()
        {
            _replay = await Client.AddUsersAsync(_userRequest);
        }

        [Then(StepTitle = "Then I should create a user")]
        private void ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeTrue();
            _replay.ErrorCode.Should().BeNullOrEmpty();
            _replay.Description.Should().BeNullOrEmpty();

            _replay.Value.Should().NotBeNull();
            _replay.Value.Id.Should().NotBeNullOrEmpty();
            _replay.Value.Addresses.Should().BeEmpty();
            _replay.Value.Phones.Should().BeEmpty();
            _replay.Value.FirstName.Should().Be(_userRequest.FirstName);
            _replay.Value.LastNames.Should().Be(_userRequest.LastNames);
            _replay.Value.Email.Should().Be(_userRequest.Email);
            _replay.Value.BirthDate.Should().Be(_userRequest.BirthDate);
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}