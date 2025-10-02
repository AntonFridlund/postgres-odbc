namespace Models.Users;

public partial record UserModel(
  int? Id,
  string? FirstName,
  string? LastName,
  string? Username,
  string? Password
);

public partial record UserModel {
  public UserModel Normalize() => this with {
    FirstName = FirstName?.Trim(),
    LastName = LastName?.Trim(),
    Username = Username?.Trim().ToLowerInvariant()
  };
}

public partial record UserModel {
  public string? Validate() {
    if (string.IsNullOrEmpty(FirstName)) {
      return "First name is required";
    } else if (FirstName is { Length: < 2 or > 32 }) {
      return "First name must be 2-32 characters";
    } else if (!FirstName.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\'')) {
      return "First name can only contain unicode letters";
    }

    if (string.IsNullOrEmpty(LastName)) {
      return "Last name is required";
    } else if (LastName.Length is < 2 or > 32) {
      return "Last name must be 2-32 characters";
    } else if (!LastName.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\'')) {
      return "Last name can only contain unicode letters";
    }

    if (string.IsNullOrEmpty(Username)) {
      return "Username is required";
    } else if (Username.Length is < 4 or > 16) {
      return "Username must be 4-16 characters";
    } else if (!Username.All(c => char.IsBetween(c, 'a', 'z'))) {
      return "Username can only contain letters a-z";
    }

    if (string.IsNullOrEmpty(Password)) {
      return "Password is required";
    } else if (Password.Length is < 8 or > 64) {
      return "Password must be 8-64 characters";
    } else if (!Password.All(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))) {
      return "Password can only contain unicode letters, digits and symbols";
    }
    return null;
  }
};
