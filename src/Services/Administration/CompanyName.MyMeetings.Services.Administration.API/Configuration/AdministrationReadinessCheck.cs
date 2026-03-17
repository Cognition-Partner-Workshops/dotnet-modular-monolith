using System.Data.SqlClient;
using Amazon.SQS;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CompanyName.MyMeetings.Services.Administration.API.Configuration
{
    public class AdministrationReadinessCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public AdministrationReadinessCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var data = new Dictionary<string, object>();

            var dbReady = await CheckDatabaseConnectivity(data, cancellationToken);
            var sqsReady = await CheckSqsAvailability(data, cancellationToken);

            if (dbReady && sqsReady)
            {
                return HealthCheckResult.Healthy("All dependencies are available.", data);
            }

            return HealthCheckResult.Unhealthy("One or more dependencies are unavailable.", data: data);
        }

        private async Task<bool> CheckDatabaseConnectivity(
            Dictionary<string, object> data,
            CancellationToken cancellationToken)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("AdministrationConnectionString");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);
                data["database"] = "connected";
                return true;
            }
            catch (Exception ex)
            {
                data["database"] = $"unavailable: {ex.Message}";
                return false;
            }
        }

        private async Task<bool> CheckSqsAvailability(
            Dictionary<string, object> data,
            CancellationToken cancellationToken)
        {
            try
            {
                var region = _configuration["Aws:Region"] ?? "us-east-1";
                var config = new AmazonSQSConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
                };

                var serviceUrl = _configuration["Aws:ServiceUrl"];
                if (!string.IsNullOrEmpty(serviceUrl))
                {
                    config.ServiceURL = serviceUrl;
                }

                using var sqsClient = new AmazonSQSClient(config);
                await sqsClient.ListQueuesAsync(string.Empty, cancellationToken);
                data["sqs"] = "available";
                return true;
            }
            catch (Exception ex)
            {
                data["sqs"] = $"unavailable: {ex.Message}";
                return false;
            }
        }
    }
}
