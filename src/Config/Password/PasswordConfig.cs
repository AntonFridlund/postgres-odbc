using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Config.Password;

// Password hashing configuration
public static class PasswordConfig {
  private static readonly PasswordHasherOptions passwordHasherOptions = new() { IterationCount = 10_000 };
  public static readonly IOptions<PasswordHasherOptions> HashOptions = Options.Create(passwordHasherOptions);
  public static readonly string? Pepper = Environment.GetEnvironmentVariable("PASSWORD_PEPPER");
}
