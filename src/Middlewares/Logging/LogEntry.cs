namespace Middlewares.Logging;

// Represents a request log
public record LogEntry(
  DateTimeOffset Timestamp,
  string LogLevel,
  string Method,
  string Path,
  int Status,
  double Duration,
  string? Error,
  string? Stack
);
