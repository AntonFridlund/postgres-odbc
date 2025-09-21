using Services.Users;

namespace Controllers.Users;

public static class UserController {
  public static async Task<IResult> GetUser(int id, IUserService userService) {
    if (id <= 0) return Results.BadRequest(new { Error = "Invalid user id" });
    var user = await userService.GetUserByIdAsync(id);
    if (user is null) return Results.NotFound(new { Error = "User not found" });
    else return Results.Ok(user);
  }

  public static async Task<IResult> CreateUser(string id, IUserService userService) {
    await Task.Yield();
    if (string.IsNullOrEmpty(id)) {
      return Results.BadRequest(new { Error = "Missing id" });
    }
    return Results.Ok(new { Message = $"User '{id}' created 🎉" });
  }
}
