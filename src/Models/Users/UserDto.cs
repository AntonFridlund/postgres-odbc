namespace Models.Users;

public partial record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Username
);

public partial record UserDto {
  public string? Validate() {
    if (string.IsNullOrWhiteSpace(FirstName)) return "First name required";
    if (string.IsNullOrWhiteSpace(LastName)) return "Last name required";
    if (string.IsNullOrWhiteSpace(Username)) return "Username required";
    return null;
  }
}
