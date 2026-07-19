using System.Collections.Concurrent;
using System.Data.Odbc;
using System.Data;

namespace Persistence.Odbc;

// Self healing connection pool
public class ConnectionPool(string connectionString, int poolSize = 10) {
  private static readonly ConcurrentDictionary<string, ConnectionPool> map = [];
  private readonly ConcurrentQueue<OdbcConnection> stack = new();
  private readonly SemaphoreSlim slot = new(poolSize, poolSize);
  private readonly string connectionString = connectionString;

  public static ConnectionPool GetPool(string connectionString, int poolSize = 10) {
    return map.GetOrAdd(connectionString, (c, p) => new ConnectionPool(c, p), poolSize);
  }

  public async Task<Lease> GetConnectionAsync() {
    await slot.WaitAsync();
    try {
      if (!stack.TryDequeue(out var connection)) connection = new(connectionString);
      if (connection.State != ConnectionState.Open) await connection.OpenAsync();
      return new Lease(this, connection);
    } catch {
      slot.Release();
      throw;
    }
  }

  public readonly struct Lease(ConnectionPool source, OdbcConnection connection) : IDisposable {
    public readonly OdbcConnection Connection = connection;
    public void Dispose() {
      if (Connection.State != ConnectionState.Open) {
        Connection.Dispose();
        source.stack.Enqueue(new(source.connectionString));
      } else source.stack.Enqueue(Connection);
      source.slot.Release();
    }
  }
}
