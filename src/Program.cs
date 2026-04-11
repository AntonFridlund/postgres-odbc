using Middlewares.Logger;
using System.Net;
using Routes;

var builder = WebApplication.CreateEmptyBuilder(new() { Args = args });
var host = Environment.GetEnvironmentVariable("APP_HOST") ?? "0.0.0.0";
var port = Environment.GetEnvironmentVariable("APP_PORT") ?? "8080";

// Add server options
builder.WebHost.UseKestrel(options => {
  options.Limits.MaxRequestBodySize = 2 * 1024 * 1024;
  options.Limits.MaxRequestHeadersTotalSize = 64 * 1024;
  options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(6);
  options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
  options.Listen(IPAddress.Parse(host), int.Parse(port));
});

// Start background logger
var logTask = LogQueue<LogEntry>.RunAsync();

// Initialize main router
builder.Services.AddRouting();
var mainRouter = new MainRouter();

// Build web server
var app = builder.Build();

// Application pipeline
app.UseMiddleware<Logger>();
mainRouter.Register(app);

// Configure graceful shutdown
app.Lifetime.ApplicationStopping.Register(() => {
  LogQueue<LogEntry>.Writer.TryComplete();
});

var appTask = app.RunAsync();

// Ensure shutdown order
await appTask;
await logTask;
