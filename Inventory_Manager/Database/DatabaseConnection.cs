using Npgsql;

namespace Inventory_Manager.Database
{
    public class DatabaseConnection
    {
        private static DatabaseConnection? _instance;

        private static string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=inventory";

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {

                    using var conn = new NpgsqlConnection(connectionString);
                    conn.Open();

                    using var cmd = new NpgsqlCommand("SELECT version()", conn);
                    string version = cmd.ExecuteScalar()?.ToString();

                    Console.WriteLine($"PostgreSQL version: {version}");

                    _instance = new();
                }

                return _instance;
            }
        }

        private DatabaseConnection() { }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
