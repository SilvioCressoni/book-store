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

namespace Users.Acceptance.Test.Scenes.Address.Add
{
    [Story(
        IWant = "Try add address to user"
    )]
    public class AddAddressWithError : BaseScene
    {
        private AddAddressRequest _request;
        private AddAddressReplay _replay;

        private ErrorResult _error;
        private string _userId;
        private int _number;
        private string _line;
        private string _postCode;

        public AddAddressWithError()
        {
            _number = Math.Abs(Fixture.Create<int>());
            _line = Fixture.Create<string>();
            _postCode = Fixture.Create<string>().Substring(0, 10);
        }

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

        [When(StepTitle = "When I try to add address")]
        private async Task WhenITryToAddAddress()
        {
            _request = Fixture.Build<AddAddressRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.Number, _number)
                .With(x => x.Line, _line)
                .With(x => x.PostCode, _postCode)
                .Create();
            
            _replay = await Client.AddAddressAsync(_request);
        }

        [Then(StepTitle = "Then I should get an error")]
        private async Task ThenIShouldGetAnError()
        {
            _replay.IsSuccess.Should().BeFalse();
            _replay.ErrorCode.Should().Be(_error.ErrorCode);
            _replay.Description.Should().Be(_error.Description);


            var user = await Client.GetUserAsync(new GetUserRequest
            {
                UserId = _userId
            });

            user.IsSuccess.Should().BeTrue();
            user.Value.Addresses.Should().BeEmpty();
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
        public void MissingLine()
        {
            _line = string.Empty;
            _error = DomainError.AddressError.MissingLine;
            this.BDDfy();
        }
        
        [Theory]
        [InlineData(101)]
        public void InvalidLine(int length)
        {
            _line = CreateText(length);
            _error = DomainError.AddressError.InvalidLine;
            this.BDDfy();
        }
        
        [Fact]
        public void InvalidNumber()
        {
            _number = -Math.Abs(Fixture.Create<int>());
            _error = DomainError.AddressError.InvalidNumber;
            this.BDDfy();
        }
        
        [Fact]
        public void MissingPostCode()
        {
            _postCode = string.Empty;
            _error = DomainError.AddressError.MissingPostCode;
            this.BDDfy();
        }
        
        [Theory]
        [InlineData(11)]
        public void InvalidPostCode(int length)
        {
            _postCode = CreateText(length);
            _error = DomainError.AddressError.InvalidPostCode;
            this.BDDfy();
        }
    }
}