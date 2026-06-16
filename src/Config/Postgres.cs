using System.Data.Odbc;

namespace Config;

// Postgres connection configuration
public static class Postgres {
    public static string ConnectionString() {
        return new OdbcConnectionStringBuilder {
            ["Driver"] = "PostgreSQL Unicode",
            ["Port"] = Environment.GetEnvironmentVariable("PG_PORT") ?? "5432",
            ["Server"] = Environment.GetEnvironmentVariable("PG_HOST") ?? "pgdb",
            ["Database"] = Environment.GetEnvironmentVariable("PG_DATABASE") ?? "postgres",
            ["Username"] = Environment.GetEnvironmentVariable("PG_USERNAME") ?? "liquibase",
            ["Password"] = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "liquibase"
        }.ConnectionString;
    }
}
