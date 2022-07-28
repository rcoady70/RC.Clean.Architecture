using RC.CA.Application.Features.Club.Queries;

namespace RC.CA.WebApi.Tests.Nunit.Features.Club.Handlers
{

    public class CreateMemberRequestHandler : TestBase
    {
        [Test]
        public async Task ShouldCreateOk()
        {
            //Arrange
            var command = Mock.Of<CreateMemberRequest>();
            command.Name = "New user";
            command.Gender = "Male";
            command.Qualification = "L3 Instructor";
            command.PhotoUrl = null;
            command.ProfilePhoto = null;

            //Act
            var result = await TestingHelper.SendAsync(command);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.ValidationErrors.Should().HaveCount(0);
            result.Value.Should().NotBe(Guid.Empty);
        }
        [Test]
        public async Task ShouldFailRequiredFields()
        {
            var command = new CreateMemberRequest();
            command.Name = "";
            command.Gender = "";
            command.Qualification = "";
            command.PhotoUrl = null;
            command.ProfilePhoto = null;

            //Act
            var result = await TestingHelper.SendAsync(command);

            //Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors.Should().HaveCountGreaterThan(0);
        }
    }
}
