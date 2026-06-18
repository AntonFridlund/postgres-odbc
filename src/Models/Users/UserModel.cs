using System.ComponentModel.DataAnnotations;

namespace Models.Users;

// Represents incoming user data
public class UserModel {
  public long? Id { get; set; }

  [Required, StringLength(32, MinimumLength = 2)]
  public string? FirstName { get; set; }

  [Required, StringLength(32, MinimumLength = 2)]
  public string? LastName { get; set; }

  [Required, StringLength(16, MinimumLength = 4)]
  public string? Username { get; set; }

  [Required, StringLength(64, MinimumLength = 8)]
  public string? Password { get; set; }

  public UserModel Normalize() {
    FirstName = FirstName?.Trim();
    LastName = LastName?.Trim();
    Username = Username?.Trim();
    return this;
  }
}
