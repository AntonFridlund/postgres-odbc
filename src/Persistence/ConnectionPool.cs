using System.Collections.Concurrent;
using System.Data.Odbc;
using System.Data;
using Config.Database;

namespace Persistence;

// Pooled odbc connections
public static class ConnectionPool {
  private static readonly int maxPool = 5;
  private static readonly SemaphoreSlim queue = new(maxPool, maxPool);
  private static readonly ConcurrentStack<OdbcConnection> pool = new();
  private static readonly string connectionString = PostgresConfig.ConnectionString();

  public static async Task<Pooled> GetConnectionAsync() {
    await queue.WaitAsync();
    try {
      if (!pool.TryPop(out var connection)) connection = new(connectionString);
      if (connection.State != ConnectionState.Open) await connection.OpenAsync();
      return new Pooled(connection);
    } catch {
      queue.Release();
      throw;
    }
  }

  public readonly struct Pooled(OdbcConnection connection) : IDisposable {
    public readonly OdbcConnection Connection = connection;
    void IDisposable.Dispose() {
      if (Connection.State != ConnectionState.Open) Connection.Dispose();
      else pool.Push(Connection);
      queue.Release();
    }
  }
}
