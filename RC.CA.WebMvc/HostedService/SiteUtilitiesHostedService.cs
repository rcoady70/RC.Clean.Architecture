using RC.CA.Application.Contracts.Identity;

namespace RC.CA.WebMvc.HostedService
{
    /// <summary>
    /// Sample hosted service to remove old log files
    /// </summary>
    public class SiteUtilitiesHostedService : BackgroundService
    {
        Timer _timer;
        readonly ILogger<SiteUtilitiesHostedService> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        private int _refreshIntervalInhours;

        public SiteUtilitiesHostedService(ILogger<SiteUtilitiesHostedService> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Delete old log files
        /// </summary>
        /// <param name="state"></param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Starting log file cleanup {DateTime.Now} {nameof(SiteUtilitiesHostedService)}.{nameof(ExecuteAsync)}");
                foreach (var file in System.IO.Directory.GetFiles($"{_hostEnvironment.ContentRootPath}/Logs"))
                {
                    try
                    {
                        var createdon = System.IO.File.GetCreationTimeUtc(file);
                        if ((DateTime.UtcNow - createdon).TotalDays > 2)
                            System.IO.File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Delete log file {file} failed with message {ex.Message} {nameof(SiteUtilitiesHostedService)}.{nameof(ExecuteAsync)}");
                    }
                }
                //
                //DateTime.Today gives time of midnight 00.00
                var nextRunTime = (DateTime.Today.AddDays(1).AddHours(1) - DateTime.Now).TotalHours;
                await Task.Delay(TimeSpan.FromHours(nextRunTime), stoppingToken);
            }
        } 
    }
}
