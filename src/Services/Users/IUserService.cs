using Models.Users;

namespace Services.Users;

// Represents a user service
public interface IUserService {
  Task<long?> CreateUserAsync(UserRequest user);
  Task<UserResponse?> GetUserByIdAsync(long id);
  Task<UserResponse?> GetUserByUsernameAsync(string username);
  Task<long?> UpdateUserAsync(long id, UserRequest user);
  Task<long?> DeleteUserAsync(long id);
}
