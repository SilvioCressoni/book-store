using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using Google.Protobuf.WellKnownTypes;
using TestStack.BDDfy;
using Users.Domain;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.Phones.Remove
{
    [Story(
        IWant = "Try remove phone when the number not exist"
    )]
    public class RemovePhoneWhenTheNumberNotExist : BaseScene
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

        [When(StepTitle = "When I remove the phone")]
        private async Task WhenIRemoveThePhone()
        {
            _request = Fixture.Build<RemovePhoneRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Number, Fixture.Create<string>())
                .Create();
            
            _replay = await Client.RemovePhoneAsync(_request);
        }

        [Then(StepTitle = "I should get number not found")]
        private void ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(DomainError.PhoneError.NumberNotFound.ErrorCode);
            _replay.Description.Should().Be(DomainError.PhoneError.NumberNotFound.Description);
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}