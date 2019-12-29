using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using Google.Protobuf.WellKnownTypes;
using TestStack.BDDfy;
using Users.Web.Proto;
using Xunit;

namespace Users.Acceptance.Test.Scenes.Phones.Add
{
    [Story(
        IWant = "Add phone to user"
    )]
    public class AddPhoneWithSuccess : BaseScene
    {
        private AddPhoneRequest _request;
        private AddPhoneReplay _replay;
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

        [When(StepTitle = "When I add the phone")]
        private async Task WhenIUpdateUserInfo()
        {
            _request = Fixture.Build<AddPhoneRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Number, Fixture.Create<string>().Substring(0, 15))
                .Create();
            
            _replay = await Client.AddPhoneAsync(_request);
        }

        [Then(StepTitle = "Then the User should have a phone")]
        private async Task ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeTrue();
            _replay.ErrorCode.Should().BeNullOrEmpty();
            _replay.Description.Should().BeNullOrEmpty();

            _replay.Value.Should().NotBeNull();
            _replay.Value.Number.Should().Be(_request.Number);

            var user = await Client.GetUserAsync(new GetUserRequest
            {
                UserId = _userId
            });

            user.IsSuccess.Should().BeTrue();
            user.Value.Phones.Should().NotBeEmpty();
            user.Value.Phones.Should().BeEquivalentTo(new List<Phone>
            {
                new Phone
                {
                    Number = _request.Number
                }
            });
        }
        
        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}