namespace Models.Users.Validator;

// Validates user model fields
public static class UserValidator {
  public static string? Validate(UserRequest user, UserFields fields = UserFields.All) {
    if (fields.HasFlag(UserFields.FirstName))
      if (ValidateFirstName(user.FirstName) is string error) return error;

    if (fields.HasFlag(UserFields.LastName))
      if (ValidateLastName(user.LastName) is string error) return error;

    if (fields.HasFlag(UserFields.Username))
      if (ValidateUsername(user.Username) is string error) return error;

    if (fields.HasFlag(UserFields.Password))
      if (ValidatePassword(user.Password) is string error) return error;

    return null;
  }

  public static string? ValidateFirstName(string? firstName) {
    if (firstName is null || firstName.Length is < 2 or > 32) {
      return "First name needs to be 2 to 32 characters long";
    } else if (!firstName.All(c => char.IsLetter(c) || c is ' ' or '-' or '\'')) {
      return "First name can only contain unicode letters";
    }
    return null;
  }

  public static string? ValidateLastName(string? lastName) {
    if (lastName is null || lastName.Length is < 2 or > 32) {
      return "Last name needs to be 2 to 32 characters long";
    } else if (!lastName.All(c => char.IsLetter(c) || c is ' ' or '-' or '\'')) {
      return "Last name can only contain unicode letters";
    }
    return null;
  }

  public static string? ValidateUsername(string? username) {
    if (username is null || username.Length is < 4 or > 16) {
      return "Username needs to be 4 to 16 characters long";
    } else if (!username.All(c => c is (>= 'a' and <= 'z') or (>= '0' and <= '9') or '_')) {
      return "Username can only contain a-z, 0-9 and underscores";
    }
    return null;
  }

  public static string? ValidatePassword(string? password) {
    if (password is null || password.Length is < 8 or > 64) {
      return "Password needs to be 8 to 64 characters long";
    }
    return null;
  }
}
