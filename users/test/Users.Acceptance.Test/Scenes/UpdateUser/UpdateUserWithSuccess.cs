using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using Google.Protobuf.WellKnownTypes;
using TestStack.BDDfy;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.UpdateUser
{
    [Story(
        IWant = "Update user info"
    )]
    public class UpdateUserWithSuccess : BaseScene
    {
        private UpdateUserRequest _request;
        private UpdateUserReplay _replay;
        private string _userId;
        private string _email;
        
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
            _email = replay.Value.Email;
        }

        [When(StepTitle = "When I Update User Info")]
        private async Task WhenIUpdateUserInfo()
        {
            _request = Fixture.Build<UpdateUserRequest>()
                .With(x => x.Id, _userId)
                .With(x => x.BirthDate, Timestamp.FromDateTime(Fixture.Create<DateTime>().AsUtc()))
                .With(x => x.FirstName, Fixture.Create<string>().Substring(0, 20))
                .Create();
            
            _replay = await Client.UpdateUserAsync(_request);
        }

        [Then(StepTitle = "Then User info should be updated")]
        private void ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeTrue();
            _replay.ErrorCode.Should().BeNullOrEmpty();
            _replay.Description.Should().BeNullOrEmpty();

            _replay.Value.Should().NotBeNull();
            _replay.Value.Id.Should().Be(_userId);
            _replay.Value.Addresses.Should().BeEmpty();
            _replay.Value.Phones.Should().BeEmpty();
            _replay.Value.FirstName.Should().Be(_request.FirstName);
            _replay.Value.LastNames.Should().Be(_request.LastNames);
            _replay.Value.Email.Should().Be(_email);
            _replay.Value.BirthDate.Should().Be(_request.BirthDate);
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}