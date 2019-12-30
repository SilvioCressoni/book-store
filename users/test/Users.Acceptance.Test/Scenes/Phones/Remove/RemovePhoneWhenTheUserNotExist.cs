using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using TestStack.BDDfy;
using Users.Domain;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.Phones.Remove
{
    [Story(
        IWant = "Try remove phone when the number not exist"
    )]
    public class RemovePhoneWhenTheUserNotExist : BaseScene
    {
        private RemovePhoneRequest _request;
        private RemovePhoneReplay _replay;
        private string _userId;

        [Given(StepTitle = "Given an user that not exist")]
        private void GivenAUser()
        {
            _userId = Fixture.Create<string>();
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

        [Then(StepTitle = "I should get user not found")]
        private void ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(DomainError.UserError.UserNotFound.ErrorCode);
            _replay.Description.Should().Be(DomainError.UserError.UserNotFound.Description);
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}