namespace Models.Users;

public class UserModel {
  public int? Id { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Username { get; set; }
  public string? Password { get; set; }

  public UserModel Transform() {
    FirstName = FirstName.Trim();
    LastName = LastName.Trim();
    Username = Username.Trim();
    return this;
  }

  public string? Validate() {
    if (FirstName.Length is < 2 or > 32) {
      return "First name must be 2-32 characters";
    } else if (!FirstName.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\'')) {
      return "First name can only contain unicode letters";
    }

    if (LastName.Length is < 2 or > 32) {
      return "Last name must be 2-32 characters";
    } else if (!LastName.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\'')) {
      return "Last name can only contain unicode letters";
    }

    if (Username.Length is < 4 or > 16) {
      return "Username must be 4-16 characters";
    } else if (!Username.All(c => char.IsAsciiLetterLower(c) || char.IsAsciiDigit(c) || c == '_')) {
      return "Username can only contain a-z, 0-9 and underscores";
    }

    if (Password is null) {
      return "Password is required";
    } else if (Password.Length is < 8 or > 64) {
      return "Password must be 8-64 characters";
    }

    return null;
  }
}
