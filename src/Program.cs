using static System.Text.Json.Serialization.JsonIgnoreCondition;
using System.Threading.Channels;
using Middlewares.Logger;
using System.Text.Json;
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

// Start background log writer
builder.Services.AddSingleton(provider => new LogQueue<LogEntry>(
    new BoundedChannelOptions(100) { SingleReader = true },
    new JsonSerializerOptions { DefaultIgnoreCondition = WhenWritingNull }
)).AddHostedService(provider => provider.GetRequiredService<LogQueue<LogEntry>>());

// Initialize main router
builder.Services.AddRouting();
var mainRouter = new MainRouter();

// Build and run server
var app = builder.Build();
app.UseMiddleware<Logger>();
mainRouter.Register(app);
app.Run();
