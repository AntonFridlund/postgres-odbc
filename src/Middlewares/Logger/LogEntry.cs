namespace Middlewares.Logger;

public record LogEntry(
  string Timestamp,
  string LogLevel,
  string Method,
  string Path,
  int Status,
  double Duration,
  string? Error,
  string? Stack
);
