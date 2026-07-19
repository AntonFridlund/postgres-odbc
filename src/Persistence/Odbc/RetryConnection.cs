using System.Data.Odbc;

namespace Persistence.Odbc;

// Connection error retry helper
public sealed class RetryConnection(ConnectionPool pool) {
  public async Task<T> ExecuteAsync<T>(Func<OdbcConnection, Task<T>> action, int retries = 3) {
    for (var attempt = 0; ; attempt++) {
      using var lease = await pool.GetConnectionAsync();
      try {
        return await action(lease.Connection);
      } catch (Exception ex) when (attempt < retries - 1) {
        if (ex is OdbcException { Errors: var errors }) {
          if (!errors.Cast<OdbcError>().Any(e => e.SQLState.StartsWith("08"))) {
            throw;
          }
        }
        await lease.Connection.CloseAsync();
        await Task.Delay(50 << attempt);
      }
    }
  }
}
