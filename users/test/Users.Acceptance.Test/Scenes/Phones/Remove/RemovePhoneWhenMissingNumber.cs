using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using Google.Protobuf.WellKnownTypes;
using TestStack.BDDfy;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.Phones.Remove
{
    [Story(
        IWant = "Try remove phone when the number is missing"
    )]
    public class RemovePhoneWhenMissingNumber : BaseScene
    {
        private RemovePhoneRequest _request;
        private RemovePhoneReplay _replay;
        private string _userId;

        [Given(StepTitle = "Given an user")]
        private async Task GivenAUser()
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
        
        [AndGiven(StepTitle =  "With phone")]
        private async Task WithPhone()
        {
            var request = Fixture.Build<AddPhoneRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Number, Fixture.Create<string>().Substring(0, 15))
                .Create();
            
            var replay = await Client.AddPhoneAsync(request);
            replay.IsSuccess.Should().BeTrue();
        }

        [When(StepTitle = "When I remove the phone")]
        private async Task WhenIUpdateUserInfo()
        {
            _request = Fixture.Build<RemovePhoneRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Number, string.Empty)
                .Create();
            
            _replay = await Client.RemovePhoneAsync(_request);
        }

        [Then(StepTitle = "Then the User should have a phone")]
        private async Task ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeTrue();
            _replay.ErrorCode.Should().BeNullOrEmpty();
            _replay.Description.Should().BeNullOrEmpty();

            var user = await Client.GetUserAsync(new GetUserRequest
            {
                UserId = _userId
            });

            user.IsSuccess.Should().BeTrue();
            user.Value.Phones.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}