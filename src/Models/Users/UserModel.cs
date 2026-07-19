using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Config.Password;

namespace Models.Users;

// Represents incoming user data
public class UserModel {
  [Required, StringLength(32, MinimumLength = 2)]
  public string? FirstName { get; set; }
  [Required, StringLength(32, MinimumLength = 2)]
  public string? LastName { get; set; }
  [Required, StringLength(16, MinimumLength = 4)]
  public string? Username { get; set; }
  [Required, StringLength(64, MinimumLength = 8)]
  public string? Password { get; set; }

  public void Normalize() {
    FirstName = FirstName?.Trim();
    LastName = LastName?.Trim();
    Username = Username?.Trim();
  }

  public void HashPassword() {
    var hasher = new PasswordHasher<UserModel>(PasswordConfig.HashOptions);
    Password = hasher.HashPassword(this, Password + PasswordConfig.Pepper);
  }
}
