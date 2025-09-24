using Services.Users;
using Middlewares;
using Routes;

var scheme = Environment.GetEnvironmentVariable("APP_SCHEME") ?? "http";
var host = Environment.GetEnvironmentVariable("APP_HOST") ?? "0.0.0.0";
var port = Environment.GetEnvironmentVariable("APP_PORT") ?? "8080";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();

builder.WebHost.ConfigureKestrel(options => {
  options.Limits.MaxRequestBodySize = 2 * 1024 * 1024;
  options.Limits.MaxRequestHeadersTotalSize = 64 * 1024;
  options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(6);
  options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
});

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.None);

var app = builder.Build();
MainRouter.Register(app);
app.Use(Logger.Log);

app.Run($"{scheme}://{host}:{port}");
