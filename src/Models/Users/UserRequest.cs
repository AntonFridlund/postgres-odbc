namespace Models.Users;

// Represents incoming user data
public class UserRequest {
  public long? Id { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Username { get; set; }
  public string? Password { get; set; }
  public string? PasswordHash { get; set; }
}
