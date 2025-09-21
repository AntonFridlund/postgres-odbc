using Configs;
using Models.Users;
using System.Data.Odbc;

namespace Controllers.Users;

// This is dummy code and you should use an orm or query builder.

public static class UserController {
  public static async Task<IResult> GetUser(string id, HttpContext context) {
    if (string.IsNullOrEmpty(id)) {
      return Results.BadRequest(new { Error = "Missing id" });
    }

    using var pool = await Postgres.GetPoolAsync();
    await using var cmd = pool.Connection.CreateCommand();

    cmd.CommandText = @"
        WITH params AS (SELECT ? id)
        SELECT u.id, u.first_name, u.last_name, u.user_name
        FROM user_data.users u, params p
        WHERE u.id = p.id";
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = id });
    await using var reader = await cmd.ExecuteReaderAsync();

    UserDto results;
    var idOrdinal = reader.GetOrdinal("id");
    var fNameOrdinal = reader.GetOrdinal("first_name");
    var lNameOrdinal = reader.GetOrdinal("last_name");
    var unameOrdinal = reader.GetOrdinal("user_name");

    if (await reader.ReadAsync()) {
      results = new UserDto(
          reader.IsDBNull(idOrdinal) ? null : reader.GetString(idOrdinal),
          reader.IsDBNull(fNameOrdinal) ? null : reader.GetString(fNameOrdinal),
          reader.IsDBNull(lNameOrdinal) ? null : reader.GetString(lNameOrdinal),
          reader.IsDBNull(unameOrdinal) ? null : reader.GetString(unameOrdinal)
      );
      return Results.Ok(results);
    }
    return Results.NotFound();
  }

  public static async Task<IResult> CreateUser(string id, HttpContext context) {
    await Task.Yield();
    if (string.IsNullOrEmpty(id)) {
      return Results.BadRequest(new { Error = "Missing id" });
    }
    return Results.Ok(new { Message = $"User '{id}' created 🎉" });
  }
}
