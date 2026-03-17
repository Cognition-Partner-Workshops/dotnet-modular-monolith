using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CompanyName.MyMeetings.Services.Administration.API.Configuration
{
    public class AdministrationHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Administration service is running."));
        }
    }
}
