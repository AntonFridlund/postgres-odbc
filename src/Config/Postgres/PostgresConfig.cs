using System.Data.Odbc;

namespace Config.Postgres;

// Postgres connection configuration
public static class PostgresConfig {
  public static readonly string ConnectionString = new OdbcConnectionStringBuilder {
    ["Driver"] = Environment.GetEnvironmentVariable("DB_DRIVER"),
    ["Server"] = Environment.GetEnvironmentVariable("DB_SERVER"),
    ["Database"] = Environment.GetEnvironmentVariable("DB_DATABASE"),
    ["Username"] = Environment.GetEnvironmentVariable("DB_USERNAME"),
    ["Password"] = Environment.GetEnvironmentVariable("DB_PASSWORD")
  }.ConnectionString;
}
