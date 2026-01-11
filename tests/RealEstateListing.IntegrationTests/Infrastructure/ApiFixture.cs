using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace RealEstateListing.IntegrationTests.Infrastructure;

/// <summary>
/// Custom WebApplicationFactory for integration testing.
/// </summary>
public class ApiFixture : IAsyncLifetime
{
    private WebApplicationFactory<Program>? _factory;
    private ITestOutputHelper? _outputHelper;

    public HttpClient HttpClient { get; private set; } = null!;

    public WebApplicationFactory<Program> SetOutputHelper(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        return RecreateFactory();
    }

    private WebApplicationFactory<Program> RecreateFactory()
    {
        _factory?.Dispose();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(ConfigureWebHost);
        return _factory;
    }

    private void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            
            if (_outputHelper != null)
            {
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.XUnitTestOutput(_outputHelper, LogEventLevel.Verbose)
                    .CreateLogger();

                logging.AddSerilog(logger, dispose: true);
            }
        });
    }

    public ValueTask InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(ConfigureWebHost);
        HttpClient = _factory.CreateClient();
        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        HttpClient?.Dispose();
        return _factory?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}

