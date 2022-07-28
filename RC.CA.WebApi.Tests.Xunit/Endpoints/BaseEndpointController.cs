using System.Text;
using System.Text.Json;
using RC.CA.Application.Dto.Authentication;
using RC.CA.SharedKernel.Result;
using RC.CA.WebApi.Tests.Xunit.Setup;
using Xunit.Abstractions;
using static RC.CA.WebApi.Tests.Xunit.Endpoints.Routes;

namespace RC.CA.WebApi.Tests.Xunit.Endpoints
{
    public class BaseEndpointController
    {
        internal readonly ITestOutputHelper _testOutputHelper;
        private static string? _jwtAccessToken = null;
        public BaseEndpointController(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        /// <summary>
        /// Get token 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetToken()
        {
            if (string.IsNullOrEmpty(_jwtAccessToken))
            {
                HttpClient _client = new WebApiApplication(_testOutputHelper).CreateClient();
                // Arrange (done in WebApiApplication)
                var command = new LoginRequest() { UserEmail = "rcoady@gmail.com", Password = "P@ssword123", RememberMe = false, ReturnUrlX = "" };
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(Authentication.Login(), UriKind.Relative);
                message.Method = HttpMethod.Post;
                message.Content = new StringContent(JsonSerializer.Serialize(command, JsonOptionConstants.SerializerOptions), Encoding.UTF8, "application/json");

                // Act
                var response = await _client.SendAsync(message);
                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CAResult<LoginResponse>>(stringResult, JsonOptionConstants.SerializerOptions);
                _jwtAccessToken = result.Value.AccessToken;
                if (result.IsSuccess)
                    return result.Value.AccessToken;
                else
                {
                    _testOutputHelper.WriteLine($"No security token created {result.ValidationErrors[0]}");
                    return "";
                }
            }
            else
                return _jwtAccessToken;
        }
    }
}
