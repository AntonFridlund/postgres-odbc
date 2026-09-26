using System.Collections.Concurrent;
using System.Data.Odbc;
using System.Data;

namespace Persistence.Odbc;

// Odbc connection pool
public sealed class ConnectionPool(string connectionString, int poolSize = 10) {
  private static readonly ConcurrentDictionary<string, ConnectionPool> pools = [];
  private readonly SemaphoreSlim slot = new(poolSize, poolSize);
  private readonly ConcurrentQueue<OdbcConnection> queue = [];

  public static ConnectionPool GetOrAdd(string connectionString, int poolSize = 10) {
    return pools.GetOrAdd(connectionString, (c, p) => new ConnectionPool(c, p), poolSize);
  }

  public async Task<Lease> GetConnectionAsync() {
    await slot.WaitAsync();
    OdbcConnection? connection = null;
    try {
      if (!queue.TryDequeue(out connection)) connection = new(connectionString);
      if (connection.State != ConnectionState.Open) await connection.OpenAsync();
      return new Lease(this, connection);
    } catch {
      connection?.Dispose();
      slot.Release();
      throw;
    }
  }

  public sealed class Lease(ConnectionPool pool, OdbcConnection connection) : IDisposable {
    public OdbcConnection Connection { get; } = connection;
    private int disposed;
    public void Dispose() {
      if (Interlocked.Exchange(ref disposed, 1) != 0) return;
      if (Connection.State != ConnectionState.Open) Connection.Dispose();
      else pool.queue.Enqueue(Connection);
      pool.slot.Release();
    }
  }
}
