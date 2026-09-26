namespace Models.Users.Validator;

// Normalizes user model fields
public static class UserNormalizer {
  public static void Normalize(UserRequest user, UserFields fields = UserFields.All) {
    if (fields.HasFlag(UserFields.FirstName)) user.FirstName = FirstName(user.FirstName);
    if (fields.HasFlag(UserFields.LastName)) user.LastName = LastName(user.LastName);
    if (fields.HasFlag(UserFields.Username)) user.Username = Username(user.Username);
  }

  public static string? FirstName(string? firstName) {
    return firstName?.Trim();
  }

  public static string? LastName(string? lastName) {
    return lastName?.Trim();
  }

  public static string? Username(string? username) {
    return username?.Trim();
  }
}
