using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RC.CA.Application.Settings;

namespace RC.CA.WebApi.UnitTests.Configuration
{
    [Trait("Category", "Configuration")]
    public class RequiredConfiguration
    {
        private readonly IServiceProvider _serviceProvider;
        public RequiredConfiguration()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            _serviceProvider = services.BuildServiceProvider();
        }
        [Fact]
        public void Check_JwtSettings_Exist()
        {
            var snapshot = _serviceProvider.GetRequiredService<IOptions<JwtSettings>>();
            Assert.NotNull(snapshot.Value);
            Assert.NotEmpty(snapshot.Value.Key);
            Assert.True(snapshot.Value.DurationInMinutes < 15, "Jwt session token must be < 15 minutes");
            Assert.NotEmpty(snapshot.Value.Key);
        }
    }
}
