using Services.Users;
using Models.Users;

namespace Controllers.Users;

// Controller for user related requests
public class UserController(IUserService userService) {
  public async Task<IResult> GetUserById(long id) {
    var user = await userService.GetUserByIdAsync(id);
    if (user is null) return Results.NotFound(new { Error = "User not found" });
    return TypedResults.Ok(user);
  }

  public async Task<IResult> CreateUser(UserModel user) {
    user.Normalize();
    var error = UserValidator.Validate(user);
    if (error is not null) return Results.BadRequest(new { Error = error });
    user.HashPassword();
    var created = await userService.CreateUserAsync(user);
    if (created is null) return Results.InternalServerError(new { Error = "Could not create user" });
    return TypedResults.Json(new { id = created }, statusCode: 201);
  }

  public async Task<IResult> DeleteUser(long id) {
    var deleted = await userService.DeleteUserAsync(id);
    if (deleted is null) return Results.NotFound(new { Error = "User not found" });
    return TypedResults.Ok(new { id = deleted });
  }
}
