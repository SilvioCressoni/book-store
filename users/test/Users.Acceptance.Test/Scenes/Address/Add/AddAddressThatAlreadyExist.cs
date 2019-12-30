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

namespace Users.Acceptance.Test.Scenes.Address.Add
{
    [Story(
        IWant = "Try to add address to user and that already exist"
    )]
    public class AddAddressThatAlreadyExist : BaseScene
    {
        private AddAddressRequest _request;
        private AddAddressReplay _replay;
        private string _userId;

        [Given(StepTitle = "Given an user")]
        private async Task GivenAnUser()
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

        [AndGiven(StepTitle = "With an address")]
        private async Task WithAnAddress()
        {
            _request = Fixture.Build<AddAddressRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.PostCode, Fixture.Create<string>().Substring(0, 10))
                .With(x => x.Number, Math.Abs(Fixture.Create<int>()))
                .Create();

            
            _replay = await Client.AddAddressAsync(_request);
            _replay.IsSuccess.Should().BeTrue();
        }

        [When(StepTitle = "When I add to add same address")]
        private async Task WhenIAddToAddSameAddress()
        {
            _replay = await Client.AddAddressAsync(_request);
        }

        [Then(StepTitle = "Then I should get an error")]
        private void ThenIShouldGetAnError()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(DomainError.AddressError.AddressAlreadyExist.ErrorCode);
            _replay.Description.Should().Be(DomainError.AddressError.AddressAlreadyExist.Description);
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}