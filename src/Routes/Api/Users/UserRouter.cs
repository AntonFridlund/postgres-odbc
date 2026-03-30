using Controllers.Users;
using Services.Users;

namespace Routes.Api.Users;

public class UserRouter {
  private static readonly UserService userService = new();
  private static readonly UserController userController = new(userService);

  public void Register(RouteGroupBuilder group) {
    var usersGroup = group.MapGroup("/users");
    usersGroup.MapGet("/{id}", userController.GetUserById);
    usersGroup.MapPost("/", userController.CreateUser);
  }
}
