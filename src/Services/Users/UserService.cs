using System.Data.Odbc;
using Models.Users;
using Configs;

namespace Services.Users;

public class UserService : IUserService {
  public async Task<UserDto?> GetUserByIdAsync(int id) {
    using var pool = await Postgres.GetPoolAsync();
    await using var cmd = pool.Connection.CreateCommand();
    cmd.CommandText = "SELECT id, first_name, last_name, user_name FROM user_data.users WHERE id = ?";
    cmd.Parameters.Add(new() { OdbcType = OdbcType.Int, Value = id });
    await using var reader = await cmd.ExecuteReaderAsync();

    var ordinals = new {
      Id = reader.GetOrdinal("id"),
      FirstName = reader.GetOrdinal("first_name"),
      LastName = reader.GetOrdinal("last_name"),
      Username = reader.GetOrdinal("user_name")
    };

    if (await reader.ReadAsync()) {
      return new UserDto(
        Id: reader.GetInt32(ordinals.Id),
        FirstName: reader.GetString(ordinals.FirstName),
        LastName: reader.GetString(ordinals.LastName),
        Username: reader.GetString(ordinals.Username)
      );
    } else return null;
  }

  public async Task<int?> CreateUserAsync(UserModel user) {
    using var pool = await Postgres.GetPoolAsync();
    await using var cmd = pool.Connection.CreateCommand();
    cmd.CommandText = "INSERT INTO user_data.users (first_name, last_name, user_name, password) VALUES (?, ?, ?, ?) RETURNING id";
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.FirstName });
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.LastName });
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.Username });
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.Password });
    return await cmd.ExecuteScalarAsync() is int id ? id : null;
  }
}
