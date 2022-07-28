using Microsoft.Extensions.Options;
using Moq;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Infrastructure.Persistence.Identity;

namespace RC.CA.WebApi.Tests.Xunit.Setup
{
    public class MockUserData
    {
        public static ApplicationUser MockUser { get; set; } = new ApplicationUser()
        {
            Id = "99999",
            FirstName = "Test",
            LastName = "User",
            RefreshToken = new List<Domain.Entities.Account.JwtRefreshToken>()
        };
        /// <summary>
        /// Match setting sin webapi application.testing.json
        /// </summary>
        public static IOptions<JwtSettings> MockJwtSettings()
        {
            var jwtsettings = Mock.Of<IOptions<JwtSettings>>();
            jwtsettings.Value.Key = "84322CFB66934ECC86D547C5CF4F2EFC";
            jwtsettings.Value.Issuer = "RC.CleanArchitecture";
            jwtsettings.Value.Audience = "RCIdentityApplicationUser";
            jwtsettings.Value.DurationInMinutes = 60;
            jwtsettings.Value.RefreshCookieName = "RCJwtRefreshCookie";
            jwtsettings.Value.RefreshExpiryInDays = 5;
            return jwtsettings;
        }
    }
}
