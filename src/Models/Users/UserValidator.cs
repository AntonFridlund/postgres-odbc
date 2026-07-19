using System.ComponentModel.DataAnnotations;

namespace Models.Users;

// Validates attributes and values
public static class UserValidator {
  public static string? Validate(UserModel user) {
    var results = new List<ValidationResult>();
    var context = new ValidationContext(user);
    if (!Validator.TryValidateObject(user, context, results, true)) {
      return results[0].ErrorMessage;
    }

    if (!user.FirstName!.All(c => char.IsLetter(c) || c is ' ' or '-' or '\'')) {
      return "First name can only contain unicode letters";
    }
    if (!user.LastName!.All(c => char.IsLetter(c) || c is ' ' or '-' or '\'')) {
      return "Last name can only contain unicode letters";
    }
    if (!user.Username!.All(c => c is (>= 'a' and <= 'z') or (>= '0' and <= '9') or '_')) {
      return "Username can only contain a-z, 0-9 and underscores";
    }
    return null;
  }
}
