using Middlewares.Logging;
using System.Net;
using Routes;

// Environment variables
var host = Environment.GetEnvironmentVariable("APP_HOST") ?? "0.0.0.0";
var port = Environment.GetEnvironmentVariable("APP_PORT") ?? "8080";

// Create new application
var builder = WebApplication.CreateEmptyBuilder(new() { Args = args });

// Configure application server
builder.WebHost.UseKestrel(options => {
  options.AddServerHeader = false;
  options.Limits.MaxRequestBodySize = 2 * 1024 * 1024;
  options.Limits.MaxRequestHeadersTotalSize = 64 * 1024;
  options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(6);
  options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
  options.Listen(IPAddress.Parse(host), int.Parse(port));
});

builder.Services.AddRouting();

// Start background logger
var logTask = LogQueue<LogEntry>.RunAsync();

// Build web server
var app = builder.Build();

// Application pipeline
app.UseMiddleware<LogWorker>();
new MainRouter().Register(app);

// Configure graceful shutdown
app.Lifetime.ApplicationStopping.Register(() => {
  LogQueue<LogEntry>.Writer.TryComplete();
});

// Start application
var appTask = app.RunAsync();

// Wait for shutdown
await appTask;
await logTask;
