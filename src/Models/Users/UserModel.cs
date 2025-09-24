namespace Models.Users;

public record UserModel(
    int Id,
    string FirstName,
    string LastName,
    string Username,
    string Password
);
