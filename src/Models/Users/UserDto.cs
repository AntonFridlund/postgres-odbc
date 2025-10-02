namespace Models.Users;

public record UserDto(
  int? Id,
  string? FirstName,
  string? LastName,
  string? Username
);
