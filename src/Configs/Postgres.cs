using System.Collections.Concurrent;
using System.Data.Odbc;
using System.Data;

namespace Configs;

public static class Postgres {
  private static readonly int MaxPool = 5;
  private static readonly SemaphoreSlim Queue = new(MaxPool, MaxPool);
  private static readonly ConcurrentStack<OdbcConnection> Pool = new();
  private static readonly string ConnectionString = new OdbcConnectionStringBuilder {
    ["Driver"] = "PostgreSQL Unicode",
    ["Port"] = Environment.GetEnvironmentVariable("DB_PORT"),
    ["Server"] = Environment.GetEnvironmentVariable("DB_SERVER"),
    ["Database"] = Environment.GetEnvironmentVariable("DB_INSTANCE"),
    ["Username"] = Environment.GetEnvironmentVariable("DB_USERNAME"),
    ["Password"] = Environment.GetEnvironmentVariable("DB_PASSWORD")
  }.ConnectionString;

  public static async Task<Pooled> GetPoolAsync() {
    await Queue.WaitAsync();
    try {
      if (!Pool.TryPop(out var connection)) connection = new(ConnectionString);
      if (connection.State != ConnectionState.Open) await connection.OpenAsync();
      return new Pooled(connection);
    } catch {
      Queue.Release();
      throw;
    }
  }

  public readonly struct Pooled(OdbcConnection connection) : IDisposable {
    public readonly OdbcConnection Connection = connection;
    void IDisposable.Dispose() {
      if (Connection.State != ConnectionState.Open) Connection.Dispose();
      else if (Pool.Count < MaxPool) Pool.Push(Connection);
      else Connection.Dispose();
      Queue.Release();
    }
  }
}
