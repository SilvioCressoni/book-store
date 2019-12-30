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

namespace Users.Acceptance.Test.Scenes.Address.Remove
{
    [Story(
        IWant = "Try remove phone when the number not exist"
    )]
    public class RemoveAddressWhenAddressIdNotFound : BaseScene
    {
        private RemoveAddressRequest _request;
        private RemoveAddressReplay _replay;
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
            var request = Fixture.Build<AddAddressRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.PostCode, Fixture.Create<string>().Substring(0, 10))
                .With(x => x.Number, Math.Abs(Fixture.Create<int>()))
                .Create();
            
            var replay = await Client.AddAddressAsync(request);
            
            replay.IsSuccess.Should().BeTrue();
        }

        [When(StepTitle = "When I remove the phone")]
        private async Task WhenIRemoveThePhone()
        {
            _request = Fixture.Build<RemoveAddressRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Id, Fixture.Create<Guid>().ToString)
                .Create();
            
            _replay = await Client.RemoveAddressAsync(_request);
        }

        [Then(StepTitle = "I should get number not found")]
        private async Task ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(DomainError.AddressError.AddressNotFound.ErrorCode);
            _replay.Description.Should().Be(DomainError.AddressError.AddressNotFound.Description);
            
            var user = await Client.GetUserAsync(new GetUserRequest
            {
                UserId = _userId
            });

            user.IsSuccess.Should().BeTrue();
            user.Value.Addresses.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}