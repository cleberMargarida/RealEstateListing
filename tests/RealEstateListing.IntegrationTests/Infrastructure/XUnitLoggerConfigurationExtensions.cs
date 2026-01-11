using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace RealEstateListing.IntegrationTests.Infrastructure;

/// <summary>
/// Extensions for configuring Serilog with xUnit test output.
/// </summary>
public static class XUnitLoggerConfigurationExtensions
{
    public static LoggerConfiguration XUnitTestOutput(
        this LoggerSinkConfiguration sinkConfiguration,
        ITestOutputHelper output,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
        IFormatProvider? formatProvider = null)
    {
        return sinkConfiguration.Sink(new XUnitLoggerSink(output, formatProvider), restrictedToMinimumLevel);
    }
}
