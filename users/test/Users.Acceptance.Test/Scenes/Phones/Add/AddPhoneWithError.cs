using System;
using System.Text;
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
        IWant = "Try add phone to user"
    )]
    public class AddPhoneWithError : BaseScene
    {
        private AddPhoneRequest _request;
        private AddPhoneReplay _replay;

        private ErrorResult _error;
        private string _userId;
        private string _number;

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
                .With(x => x.Number, _number)
                .Create();
            
            _replay = await Client.AddPhoneAsync(_request);
        }

        [Then(StepTitle = "Then the User shouldn't have a phone")]
        private async Task ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(_error.ErrorCode);
            _replay.Description.Should().Be(_error.Description);


            var user = await Client.GetUserAsync(new GetUserRequest
            {
                UserId = _userId
            });

            user.IsSuccess.Should().BeTrue();
            user.Value.Phones.Should().BeEmpty();
        }
        
        private string CreateText(int length)
        {
            var text = new StringBuilder();
            while (text.Length < length)
            {
                text.Append(Fixture.Create<string>());
            }

            return text.ToString().Substring(0, length);
        }

        [Fact]
        public void MissingPhone()
        {
            _number = string.Empty;
            _error = DomainError.PhoneError.MissingNumber;
            this.BDDfy();
        }
        
        [Theory]
        [InlineData(16)]
        public void InvalidPhone(int length)
        {
            _number = CreateText(length);
            _error = DomainError.PhoneError.InvalidNumber;
            this.BDDfy();
        }
    }
}