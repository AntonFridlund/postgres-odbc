namespace Models.Users;

// Represents outgoing user data
public record UserDto(
  long Id,
  string? FirstName,
  string? LastName,
  string? Username
);
