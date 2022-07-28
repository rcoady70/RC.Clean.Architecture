using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.SharedKernel.Result;
using RC.CA.WebApi.Tests.Xunit.Setup;
using Xunit.Abstractions;

namespace RC.CA.WebApi.Tests.Xunit.Endpoints.Members;

[Trait("Category", "Members_Endpoint")]
public class TestMembersController : BaseEndpointController
{

    private readonly ILogger _logger;
    private readonly HttpClient _client;

    public TestMembersController(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _logger = XUnitLogger.CreateLogger<Swagger>(_testOutputHelper);
        _client = new WebApiApplication(_testOutputHelper).CreateClient();
    }
    [Fact]
    public async Task Get_Member_ValidId_OK200()
    {
        // Arrange (done in WebApiApplication)
        var command = new GetMemberRequest() { Id = new Guid("6e4a2722-0e91-44fd-8a00-048a1ec7a24c") };
        string jwtToken = await this.GetToken();
        HttpRequestMessage message = new HttpRequestMessage();
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Authorization", $"Bearer {jwtToken}");
        message.RequestUri = new Uri(Routes.Members.Get(), UriKind.Relative);
        message.Method = HttpMethod.Get;
        message.Content = new StringContent(JsonSerializer.Serialize(command, JsonOptionConstants.SerializerOptions), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(message);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CAResult<GetMemberResponseDto>>(stringResult, JsonOptionConstants.SerializerOptions);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Value);
        Assert.Equal(result.IsSuccess, true);
        Assert.NotEqual(result.Value.Experiences.Count, 0);
    }
    [Fact]
    public async Task Create_Member_ValidationFail_BadRequest400()
    {
        // Arrange (done in WebApiApplication)
        var command = new CreateMemberRequest();
        HttpRequestMessage message = new HttpRequestMessage();
        string jwtToken = await this.GetToken();
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Authorization", $"Bearer {jwtToken}");
        message.RequestUri = new Uri(Routes.Members.Create(), UriKind.Relative);
        message.Method = HttpMethod.Put;
        message.Content = new StringContent(JsonSerializer.Serialize(command, JsonOptionConstants.SerializerOptions), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(message);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CAResult<CreateMemberResponseDto>>(stringResult, JsonOptionConstants.SerializerOptions);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.IsSuccess, false);
        Assert.NotEqual(result.ValidationErrors.Count, 0);
    }
    [Fact]
    public async Task Create_Member_Valid_OK200()
    {
        // Arrange (done in WebApiApplication)
        string jwtToken = await this.GetToken();
        var command = MockMemberData.MockMember;
        command.Id = new Guid();
        HttpRequestMessage message = new HttpRequestMessage();
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Authorization", $"Bearer {jwtToken}");
        message.RequestUri = new Uri(Routes.Members.Create(), UriKind.Relative);
        message.Method = HttpMethod.Put;
        message.Content = new StringContent(JsonSerializer.Serialize(command, JsonOptionConstants.SerializerOptions), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(message);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CAResult<CreateMemberResponseDto>>(stringResult, JsonOptionConstants.SerializerOptions);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Value);
        Assert.Equal(result.IsSuccess, true);
        Assert.Equal(result.ValidationErrors.Count, 0);
    }
}
