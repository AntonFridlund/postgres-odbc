using System.Data.Odbc;

namespace Config.Postgres;

// Postgres connection configuration
public static class PostgresConfig {
    public static readonly string ConnectionString = new OdbcConnectionStringBuilder {
        ["Driver"] = "PostgreSQL Unicode",
        ["Port"] = Environment.GetEnvironmentVariable("PG_PORT") ?? "5432",
        ["Server"] = Environment.GetEnvironmentVariable("PG_HOST") ?? "pgdb",
        ["Database"] = Environment.GetEnvironmentVariable("PG_DATABASE") ?? "postgres",
        ["Username"] = Environment.GetEnvironmentVariable("PG_USERNAME") ?? "liquibase",
        ["Password"] = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "liquibase"
    }.ConnectionString;
}
