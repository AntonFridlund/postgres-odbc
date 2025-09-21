using System.Data.Odbc;
using Models.Users;
using Configs;

namespace Services.Users;

public interface IUserService {
  Task<UserDto?> GetUserByIdAsync(int id);
}

public class UserService : IUserService {
  public async Task<UserDto?> GetUserByIdAsync(int id) {
    using var pool = await Postgres.GetPoolAsync();
    await using var cmd = pool.Connection.CreateCommand();

    cmd.CommandText = @"
            WITH params AS (SELECT ? id)
            SELECT u.id, u.first_name, u.last_name, u.user_name
            FROM user_data.users u, params p
            WHERE u.id = p.id";
    cmd.Parameters.Add(new() { OdbcType = OdbcType.Int, Value = id });
    await using var reader = await cmd.ExecuteReaderAsync();

    var usrIdOrdinal = reader.GetOrdinal("id");
    var fNameOrdinal = reader.GetOrdinal("first_name");
    var lNameOrdinal = reader.GetOrdinal("last_name");
    var unameOrdinal = reader.GetOrdinal("user_name");

    if (await reader.ReadAsync()) {
      return new UserDto(
        reader.IsDBNull(usrIdOrdinal) ? null : reader.GetInt32(usrIdOrdinal),
        reader.IsDBNull(fNameOrdinal) ? null : reader.GetString(fNameOrdinal),
        reader.IsDBNull(lNameOrdinal) ? null : reader.GetString(lNameOrdinal),
        reader.IsDBNull(unameOrdinal) ? null : reader.GetString(unameOrdinal)
        );
    } else return null;
  }
}
