using System.Data.Odbc;
using Models.Users;
using Configs;

namespace Services.Users;

public class UserService : IUserService {
  public async Task<UserDto?> GetUserByIdAsync(int id) {
    using var pool = await Postgres.GetPoolAsync();
    await using var cmd = pool.Connection.CreateCommand();
    cmd.CommandText = @"
      SELECT id, first_name, last_name, user_name
      FROM user_data.users
      WHERE id = ?";
    cmd.Parameters.Add(new() { OdbcType = OdbcType.Int, Value = id });
    await using var reader = await cmd.ExecuteReaderAsync();

    var usrIdOrdinal = reader.GetOrdinal("id");
    var fNameOrdinal = reader.GetOrdinal("first_name");
    var lNameOrdinal = reader.GetOrdinal("last_name");
    var unameOrdinal = reader.GetOrdinal("user_name");

    if (await reader.ReadAsync()) {
      return new UserDto(
        reader.GetInt32(usrIdOrdinal),
        reader.GetString(fNameOrdinal),
        reader.GetString(lNameOrdinal),
        reader.GetString(unameOrdinal)
      );
    } else return null;
  }
}
