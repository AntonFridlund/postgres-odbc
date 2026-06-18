using Services.Users;
using Models.Users;

namespace Controllers.Users;

// Controller for user related requests
public class UserController(IUserService userService) {
  public async Task<IResult> GetUserById(long id) {
    if (id <= 0) return Results.BadRequest(new { Error = "Invalid user id" });
    var user = await userService.GetUserByIdAsync(id);
    if (user is null) return Results.NotFound(new { Error = "User not found" });
    return Results.Json(data: user, statusCode: 200);
  }

  public async Task<IResult> CreateUser(UserModel user) {
    var error = UserValidation.Validate(user.Normalize());
    if (error is not null) return Results.BadRequest(new { Error = error });
    var id = await userService.CreateUserAsync(user);
    if (id is null) return Results.InternalServerError(new { Error = "Could not create user" });
    return Results.Json(data: new { id }, statusCode: 201);
  }
}
