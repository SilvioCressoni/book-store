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

namespace Users.Acceptance.Test.Scenes.Address.Add
{
    [Story(
        IWant = "Add phone to user"
    )]
    public class AddAddressWithSuccess : BaseScene
    {
        private AddAddressRequest _request;
        private AddAddressReplay _replay;
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
            _request = Fixture.Build<AddAddressRequest>()
                .With(x => x.UserId, _userId)
                .With(x => x.PostCode, Fixture.Create<string>().Substring(0, 10))
                .With(x => x.Number, Math.Abs(Fixture.Create<int>()))
                .Create();
            
            _replay = await Client.AddAddressAsync(_request);
        }

        [Then(StepTitle = "Then the User should have a phone")]
        private async Task ThenIShouldCreateAUser()
        {
            _replay.IsSuccess.Should().BeTrue();
            _replay.ErrorCode.Should().BeNullOrEmpty();
            _replay.Description.Should().BeNullOrEmpty();

            _replay.Value.Should().NotBeNull();
            _replay.Value.Id.Should().NotBeNullOrEmpty();
            _replay.Value.Should().BeEquivalentTo(new Web.Proto.Address
            {
                Id = _replay.Value.Id,
                Line = _request.Line,
                PostCode = _request.PostCode,
                Number = _request.Number,
            });

            var user = await Client.GetUserAsync(new GetUserRequest
            {
                UserId = _userId
            });

            user.IsSuccess.Should().BeTrue();
            user.Value.Addresses.Should().NotBeEmpty();
            user.Value.Addresses.Should().BeEquivalentTo(new List<Web.Proto.Address>
            {
                new Web.Proto.Address()
                {
                    Id = _replay.Value.Id,
                    Line = _request.Line,
                    PostCode = _request.PostCode,
                    Number = _request.Number,
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