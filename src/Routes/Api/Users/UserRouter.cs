using Controllers.Users;
using Services.Users;

namespace Routes.Api.Users;

// User related endpoint
public class UserRouter {
  private static readonly UserService userService = new();
  private static readonly UserController userController = new(userService);

  public void Register(RouteGroupBuilder group) {
    var usersGroup = group.MapGroup("/users");
    usersGroup.MapGet("/{id:long}", userController.GetUserById);
    usersGroup.MapPost("/", userController.CreateUser);
    usersGroup.MapDelete("/{id:long}", userController.DeleteUser);
  }
}
