using Controllers.Users;

namespace Routes.Api.V1.Users;

public static class UserRouter {
  public static void Register(RouteGroupBuilder group) {
    var users = group.MapGroup("/users");
    users.MapGet("/{id}", UserController.GetUser);
    users.MapPost("/", UserController.CreateUser);
  }
}
