namespace RC.CA.WebApi.Tests.Nunit;
public class TestBase
{
    [SetUp]
    public async Task SetUp()
    {
        await TestingHelper.ResetState();
    }
}
