namespace Models.Users;

public record UserModel(
    string? Id,
    string? FirstName,
    string? LastName,
    string? Username,
    string? Password
);
