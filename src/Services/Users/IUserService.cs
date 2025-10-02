using Models.Users;

namespace Services.Users;

public interface IUserService {
  Task<UserDto?> GetUserByIdAsync(int id);
  Task<int?> CreateUserAsync(UserModel userModel);
}
