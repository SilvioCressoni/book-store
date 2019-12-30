using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using TestStack.BDDfy;
using Users.Domain;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.Address.Get
{
    [Story(
        IWant = "Add phone to user"
    )]
    public class GetAddressWhenUserNotFound : BaseScene
    {
        private GetAddressesRequest _request;
        private GetAddressesReplay _replay;
        private string _userId;
        [Given(StepTitle = "Given an user that not exist")]
        private void GivenAnUser()
        {
            _userId = Fixture.Create<string>();
        }

        [When(StepTitle = "When I get user number")]
        private async Task WhenIUpdateUserInfo()
        {
            _request = Fixture.Build<GetAddressesRequest>()
                .With(x => x.UserId, _userId)
                .Create();
            
            _replay = await Client.GetAddressesAsync(_request);
        }

        [Then(StepTitle = "Then I should get user not found")]
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