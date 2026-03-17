using Autofac;
using Autofac.Extensions.DependencyInjection;
using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Domain;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus;
using CompanyName.MyMeetings.Modules.Administration.Application.Contracts;
using CompanyName.MyMeetings.Modules.Administration.Infrastructure;
using CompanyName.MyMeetings.Modules.Administration.Infrastructure.Configuration;
using CompanyName.MyMeetings.Services.Administration.API.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace CompanyName.MyMeetings.Services.Administration.API
{
    public class Startup
    {
        private const string AdministrationConnectionString = "AdministrationConnectionString";
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            ConfigureLogger();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables("Administration_")
                .Build();

            _loggerForApi.Information(
                "Administration Service starting. Connection string configured: {HasConnectionString}",
                !string.IsNullOrEmpty(_configuration.GetConnectionString(AdministrationConnectionString)));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Administration Service API",
                    Version = "v1",
                    Description = "Standalone Administration bounded context API extracted from the MyMeetings modular monolith."
                });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

            services.AddHealthChecks()
                .AddCheck<AdministrationHealthCheck>("administration_health")
                .AddCheck<AdministrationReadinessCheck>("administration_readiness");
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AdministrationModule>()
                .As<IAdministrationModule>()
                .InstancePerLifetimeScope();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            var container = app.ApplicationServices.GetAutofacRoot();

            InitializeModule(container);

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Administration Service API v1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Name == "administration_health"
                });
                endpoints.MapHealthChecks("/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Name == "administration_readiness"
                });
            });
        }

        private static void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(new CompactJsonFormatter(), "logs/administration-service")
                .CreateLogger();

            _loggerForApi = _logger.ForContext("Module", "Administration.API");

            _loggerForApi.Information("Logger configured");
        }

        private void InitializeModule(ILifetimeScope container)
        {
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

            var snsEventsBusConfig = new SnsEventsBusConfiguration
            {
                Region = _configuration["Aws:Region"] ?? "us-east-1",
                TopicArnPrefix = _configuration["Aws:SnsTopicArnPrefix"] ?? string.Empty,
                ServiceUrl = _configuration["Aws:ServiceUrl"]
            };

            var eventsBus = new SnsEventsBus(_logger, snsEventsBusConfig);

            AdministrationStartup.Initialize(
                _configuration.GetConnectionString(AdministrationConnectionString),
                executionContextAccessor,
                _logger,
                eventsBus);
        }
    }
}
