using Serilog.Core;
using Serilog.Events;

namespace RealEstateListing.IntegrationTests.Infrastructure;

/// <summary>
/// Custom Serilog sink that writes to xUnit's ITestOutputHelper.
/// Compatible with xUnit v3.
/// </summary>
public class XUnitLoggerSink(ITestOutputHelper output, IFormatProvider? formatProvider = null) : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(formatProvider);
        var formattedMessage = $"[{logEvent.Timestamp:HH:mm:ss} {logEvent.Level}] {message}";

        if (logEvent.Exception != null)
        {
            formattedMessage += Environment.NewLine + logEvent.Exception;
        }

        output.WriteLine(formattedMessage);
    }
}
