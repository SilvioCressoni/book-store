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

namespace Users.Acceptance.Test.Scenes.Phones.Add
{
    [Story(
        IWant = "Try to add phone to user that already exist"
    )]
    public class AddPhoneThatAlreadyExist : BaseScene
    {
        private AddPhoneRequest _request;
        private AddPhoneReplay _replay;
        private string _number;
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

        [AndGiven(StepTitle = "And this with phone")]
        private async Task AndWithPhone()
        {
            _request = Fixture.Build<AddPhoneRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Number, Fixture.Create<string>().Substring(0, 15))
                .Create();
            
            _replay = await Client.AddPhoneAsync(_request);
            _replay.IsSuccess.Should().BeTrue();
            _number = _replay.Value.Number;
        }

        [When(StepTitle = "When I add the phone")]
        private async Task WhenIUpdateUserInfo()
        {
            _request = Fixture.Build<AddPhoneRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Number, _number)
                .Create();
            
            _replay = await Client.AddPhoneAsync(_request);
        }

        [Then(StepTitle = "Then I should get a error")]
        private void ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(DomainError.PhoneError.NumberAlreadyExist.ErrorCode);
            _replay.Description.Should().Be(DomainError.PhoneError.NumberAlreadyExist.Description);
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}