using System.Data;
using System.Data.Odbc;

namespace Persistence.Odbc;

// Connection error retry
public sealed class OdbcConnectionRetry(string connectionString) {
  public async Task<T> ExecuteAsync<T>(Func<OdbcConnection, Task<T>> action, int attempts = 5) {
    for (var attempt = 0; ; attempt++) {
      try {
        using var connection = new OdbcConnection(connectionString);
        await connection.OpenAsync();
        return await action(connection);
      } catch (Exception ex) when (attempt < attempts - 1) {
        if (ex.GetBaseException() is not OdbcException { Errors: var errors }) throw;
        if (!errors.Cast<OdbcError>().Any(e => e.SQLState.StartsWith("08"))) throw;
        await Task.Delay(50 << Math.Min(attempt, 4));
      }
    }
  }
}
