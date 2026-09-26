using System.Data;
using System.Data.Odbc;

namespace Persistence.Odbc;

// Connection error retry
public sealed class ConnectionRetry(ConnectionPool pool) {
  public async Task<T> ExecuteAsync<T>(Func<OdbcConnection, Task<T>> action, int retries = 3) {
    ArgumentOutOfRangeException.ThrowIfLessThan(retries, 1);
    for (var attempt = 0; ; attempt++) {
      try {
        using var lease = await pool.GetConnectionAsync();
        return await action(lease.Connection);
      } catch (Exception ex) when (attempt < retries - 1) {
        if (ex.GetBaseException() is not OdbcException { Errors: var errors }) throw;
        if (!errors.Cast<OdbcError>().Any(e => e.SQLState.StartsWith("08"))) throw;
        await Task.Delay(50 << attempt);
      }
    }
  }
}
