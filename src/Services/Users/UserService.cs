using System.Data.Odbc;
using Models.Users;
using Persistence;

namespace Services.Users;

// User related data source communication
public class UserService : IUserService {
  public async Task<UserDto?> GetUserByIdAsync(long id) {
    using var pooled = await ConnectionPool.GetConnectionAsync();
    await using var cmd = pooled.Connection.CreateCommand();
    cmd.CommandText = "SELECT id, first_name, last_name, user_name FROM user_data.users WHERE id = ?";
    cmd.Parameters.Add(new() { OdbcType = OdbcType.BigInt, Value = id });
    await using var reader = await cmd.ExecuteReaderAsync();
    if (!await reader.ReadAsync()) return null;
    var row = new ResponseReader(reader);
    return new UserDto(
      Id: row.Required<long>(reader.GetOrdinal("id")),
      FirstName: row.Nullable<string?>(reader.GetOrdinal("first_name")),
      LastName: row.Nullable<string?>(reader.GetOrdinal("last_name")),
      Username: row.Nullable<string?>(reader.GetOrdinal("user_name"))
    );
  }

  public async Task<long?> CreateUserAsync(UserModel user) {
    using var pooled = await ConnectionPool.GetConnectionAsync();
    await using var cmd = pooled.Connection.CreateCommand();
    cmd.CommandText = "INSERT INTO user_data.users (first_name, last_name, user_name, password) VALUES (?, ?, ?, ?) RETURNING id";
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.FirstName });
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.LastName });
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.Username });
    cmd.Parameters.Add(new() { OdbcType = OdbcType.VarChar, Value = user.Password });
    return await cmd.ExecuteScalarAsync() is long id ? id : null;
  }
}
