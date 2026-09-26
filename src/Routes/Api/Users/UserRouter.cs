using Controllers.Users;
using Services.Users;

namespace Routes.Api.Users;

// User related endpoint
public class UserRouter {
  private static readonly UserService userService = new();
  private static readonly UserController userController = new(userService);

  public void Register(RouteGroupBuilder group) {
    var usersGroup = group.MapGroup("/users");
    usersGroup.MapPost("/", userController.CreateUser);
    usersGroup.MapGet("/{id:long}", userController.GetUserById);
    usersGroup.MapGet("/{username}", userController.GetUserByUsername);
    usersGroup.MapPut("/{id:long}", userController.UpdateUser);
    usersGroup.MapDelete("/{id:long}", userController.DeleteUser);
  }
}
