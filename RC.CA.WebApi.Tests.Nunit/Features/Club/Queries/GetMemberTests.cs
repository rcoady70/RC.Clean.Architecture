using RC.CA.Application.Features.Club.Queries;

namespace RC.CA.WebApi.Tests.Nunit.Features.Club.Queries;

public class GetMemberTests : TestBase
{
    [Test]
    public async Task ShouldGetAllMembersAndAssociatedExpierences()
    {
        //Arrange add the record you are going to get
        var getMemberRequestQuery = new GetMemberRequest() { Id = MockMemberData.MockMember.Id };
        await TestingHelper.AddAsync(MockMemberData.MockMember);

        //Act
        var result = await TestingHelper.SendAsync(getMemberRequestQuery);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Experiences.Should().NotBeNull();
        result.Value.Experiences.Should().HaveCount(2);
    }
}
