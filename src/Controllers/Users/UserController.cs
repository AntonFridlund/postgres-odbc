using Models.Users;
using Services.Users;

namespace Controllers.Users;

public static class UserController {
  public static async Task<IResult> GetUser(int id, IUserService userService) {
    try {
      if (id <= 0) return Results.BadRequest(new { Error = "Invalid user id" });
      var user = await userService.GetUserByIdAsync(id);
      if (user is null) return Results.NotFound(new { Error = "User not found" });
      else return Results.Ok(user);
    } catch {
      return Results.InternalServerError(new { Error = "Could not fetch user" });
    }
  }

  public static async Task<IResult> CreateUser(HttpRequest req, IUserService userService) {
    try {
      var user = await req.ReadFromJsonAsync<UserModel>();
      if (user is null) return Results.BadRequest(new { Error = "Invalid JSON" });
      if (user.Validate() is string error) return Results.BadRequest(new { Error = error });
      var id = await userService.CreateUserAsync(user);
      return Results.Created($"/users/{id}", new { Id = id });
    } catch {
      return Results.InternalServerError(new { Error = "Could not create user" });
    }
  }
}
