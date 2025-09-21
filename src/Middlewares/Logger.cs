using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Middlewares {
  public class Logger {
    public static async Task Log(HttpContext context, RequestDelegate next) {
      var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
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

      Console.WriteLine(
        JsonSerializer.Serialize(new {
          Timestamp = timestamp,
          LogLevel = logLevel.ToString(),
          Method = context.Request.Method,
          Path = context.Request.Path.ToString(),
          Status = context.Response.StatusCode,
          Duration = timer.Elapsed.TotalMilliseconds,
          ErrorMessage = errorMessage,
          StackTrace = stackTrace
        }, options: new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull })
      );
    }
  }
}
