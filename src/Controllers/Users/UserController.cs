using System.Text.Json;
using Models.Users;
using Services.Users;

namespace Controllers.Users;

public static class UserController {
  public static async Task<IResult> GetUser(int id, IUserService userService) {
    if (id <= 0) return Results.BadRequest(new { Error = "Invalid user id" });
    UserDto? user = await userService.GetUserByIdAsync(id);
    if (user is null) return Results.NotFound(new { Error = "User not found" });
    else return Results.Ok(user);
  }

  public static async Task<IResult> CreateUser(HttpRequest req, IUserService userService) {
    UserModel? user;
    try {
      user = await req.ReadFromJsonAsync<UserModel>();
    } catch (JsonException) {
      return Results.BadRequest(new { Error = "Invalid JSON format" });
    } catch (InvalidOperationException) {
      return Results.BadRequest(new { Error = "Unsupported content type" });
    }
    if (user is null) return Results.BadRequest(new { Error = "Invalid JSON data" });
    user = user.Normalize();
    if (user.Validate() is string error) return Results.BadRequest(new { Error = error });
    int? id = await userService.CreateUserAsync(user);
    if (id is null) return Results.InternalServerError(new { Error = "Could not create user" });
    return Results.Created($"/users/{id}", new { Id = id });
  }
}
