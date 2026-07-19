using Models.Users;

namespace Services.Users;

// Represents a user service
public interface IUserService {
  Task<UserDto?> GetUserByIdAsync(long id);
  Task<long?> CreateUserAsync(UserModel userModel);
  Task<long?> DeleteUserAsync(long id);
}
