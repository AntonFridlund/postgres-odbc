using System.Diagnostics;

namespace Middlewares.Logging;

// Creates and queues log entries from incoming requests
public class LogWorker(RequestDelegate next) {
  public async Task InvokeAsync(HttpContext context) {
    var timestamp = DateTimeOffset.UtcNow;
    var logLevel = LogLevel.Information;
    var timer = Stopwatch.StartNew();
    string? errorMessage = null;
    string? stackTrace = null;
    try {
      await next(context);
    } catch (Exception exception) {
      logLevel = LogLevel.Error;
      errorMessage = exception.Message;
      stackTrace = exception.StackTrace;
      context.Response.StatusCode = 500;
      await context.Response.WriteAsJsonAsync(new { Error = "Internal Server Error" });
    }
    timer.Stop();
    await LogQueue<LogEntry>.Writer.WriteAsync(
      new LogEntry(
        Timestamp: timestamp,
        LogLevel: logLevel.ToString(),
        Method: context.Request.Method,
        Path: context.Request.Path.ToString(),
        Status: context.Response.StatusCode,
        Duration: timer.Elapsed.TotalMilliseconds,
        Error: errorMessage,
        Stack: stackTrace
      )
    );
  }
}
