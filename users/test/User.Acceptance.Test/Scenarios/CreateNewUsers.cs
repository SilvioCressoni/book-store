using AutoFixture;
using TestStack.BDDfy;
using Xunit;

namespace Users.Acceptance.Test.Scenarios
{
    [Story(
        IWant = "Create a new user",
        SoThat = "I can use system"
        )]
    public class CreateNewUsers
    {
        private readonly Fixture _fixture;
        // private User user;
        public CreateNewUsers()
        {
            _fixture = new Fixture();
        }


        [Given(StepTitle = "Given new user")]
        private void GivenANewUser()
        {
            
        }

        [When(StepTitle = "When I Call API")]
        private void WhenICallAPI()
        {
            
        }

        [Then(StepTitle = "Then I should create a user")]
        private void ThenIShouldCreateAUser()
        {
            
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}