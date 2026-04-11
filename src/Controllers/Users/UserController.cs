using Services.Users;
using Models.Users;

namespace Controllers.Users;

public class UserController(IUserService userService) {
  public async Task<IResult> GetUserById(int id) {
    // Validate user request data
    if (id <= 0) return Results.BadRequest(new { Error = "Invalid user id" });

    // Try to fetch user by id
    var user = await userService.GetUserByIdAsync(id);
    if (user is null) return Results.NotFound(new { Error = "User not found" });
    else return Results.Ok(user);
  }

  public async Task<IResult> CreateUser(UserModel user) {
    // Validate user request data
    var error = user.Transform().Validate();
    if (error is not null) return Results.BadRequest(new { Error = error });

    // Try to create user
    var id = await userService.CreateUserAsync(user);
    if (id is null) return Results.InternalServerError(new { Error = "Could not create user" });
    return Results.Created($"/api/users/{id}", new { Id = id });
  }
}
