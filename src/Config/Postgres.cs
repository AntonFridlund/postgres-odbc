using System.Data.Odbc;

namespace Config;

// Postgres connection configuration
public static class Postgres {
    public static string ConnectionString() {
        return new OdbcConnectionStringBuilder {
            ["Driver"] = "PostgreSQL Unicode",
            ["Port"] = Environment.GetEnvironmentVariable("PG_PORT") ?? "5432",
            ["Server"] = Environment.GetEnvironmentVariable("PG_HOST") ?? "localhost",
            ["Database"] = Environment.GetEnvironmentVariable("PG_DATABASE") ?? "postgres",
            ["Username"] = Environment.GetEnvironmentVariable("PG_USERNAME") ?? "postgres",
            ["Password"] = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "postgres"
        }.ConnectionString;
    }
}
