using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using Google.Protobuf.WellKnownTypes;
using TestStack.BDDfy;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.Address.Remove
{
    [Story(
        IWant = "Add phone to user"
    )]
    public class RemoveAddressWithSuccess : BaseScene
    {
        private RemoveAddressRequest _request;
        private RemoveAddressReplay _replay;
        private string _addressId;
        private string _userId;

        [Given(StepTitle = "Given an user")]
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
        
        [AndGiven(StepTitle =  "With phone")]
        private async Task WithPhone()
        {
            var request = Fixture.Build<AddAddressRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.PostCode, Fixture.Create<string>().Substring(0, 10))
                .With(x => x.Number, Math.Abs(Fixture.Create<int>()))
                .Create();
            
            var replay = await Client.AddAddressAsync(request);
            
            replay.IsSuccess.Should().BeTrue();
            _addressId = replay.Value.Id;
        }

        [When(StepTitle = "When I remove the phone")]
        private async Task WhenIUpdateUserInfo()
        {
            _request = Fixture.Build<RemoveAddressRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Id, _addressId)
                .Create();
            
            _replay = await Client.RemoveAddressAsync(_request);
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
            user.Value.Phones.Should().BeEmpty();
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}