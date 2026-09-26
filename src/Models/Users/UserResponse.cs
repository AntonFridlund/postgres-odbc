namespace Models.Users;

// Represents outgoing user data
public record UserResponse(
  long Id,
  string? FirstName,
  string? LastName,
  string? Username
);
