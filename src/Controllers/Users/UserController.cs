using Microsoft.AspNetCore.Identity;
using Services.Users;
using Models.Users;
using Models.Users.Validator;

namespace Controllers.Users;

// Controller for user related requests
public class UserController(IUserService userService) {
  private readonly PasswordHasher<UserRequest> hasher = new();

  public async Task<IResult> CreateUser(UserRequest user) {
    UserNormalizer.Normalize(user, UserFields.Create);
    var error = UserValidator.Validate(user, UserFields.Create);
    if (error is not null) return Results.BadRequest(new { Error = error });
    user.PasswordHash = hasher.HashPassword(user, user.Password!);
    var created = await userService.CreateUserAsync(user);
    if (created is null) return Results.InternalServerError(new { Error = "Could not create user" });
    return TypedResults.Json(new { id = created }, statusCode: 201);
  }

  public async Task<IResult> GetUserById(long id) {
    var user = await userService.GetUserByIdAsync(id);
    if (user is null) return Results.NotFound(new { Error = "User not found" });
    return TypedResults.Ok(user);
  }

  public async Task<IResult> GetUserByUsername(string username) {
    var user = await userService.GetUserByUsernameAsync(username);
    if (user is null) return Results.NotFound(new { Error = "User not found" });
    return TypedResults.Ok(user);
  }

  public async Task<IResult> UpdateUser(long id, UserRequest user) {
    UserNormalizer.Normalize(user, UserFields.Put);
    var error = UserValidator.Validate(user, UserFields.Put);
    if (error is not null) return Results.BadRequest(new { Error = error });
    var updated = await userService.UpdateUserAsync(id, user);
    if (updated is null) return Results.InternalServerError(new { Error = "Could not update user" });
    return TypedResults.Ok(new { id = updated });
  }

  public async Task<IResult> DeleteUser(long id) {
    var deleted = await userService.DeleteUserAsync(id);
    if (deleted is null) return Results.NotFound(new { Error = "User not found" });
    return TypedResults.Ok(new { id = deleted });
  }
}
