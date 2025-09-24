namespace Models.Users;

public partial record UserModel(
    int Id,
    string FirstName,
    string LastName,
    string Username,
    string Password
);

public partial record UserModel {
  public string? Validate() {
    if (string.IsNullOrWhiteSpace(FirstName)) return "First name required";
    if (string.IsNullOrWhiteSpace(LastName)) return "Last name required";
    if (string.IsNullOrWhiteSpace(Username)) return "Username required";
    if (string.IsNullOrWhiteSpace(Password)) return "Password required";
    return null;
  }
}
