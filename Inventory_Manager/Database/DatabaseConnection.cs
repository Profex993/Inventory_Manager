using Npgsql;

namespace Inventory_Manager.Database
{
    public class DatabaseConnection
    {

        private static string connectionString;

        public static DatabaseConnection Instance { get; private set; }

        public DatabaseConnection(string host, string username, string password)
        {
            if (Instance == null)
            {
                connectionString = $"Host={host};Username={username};Password={password};Database=inventory";
                Instance = this;
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static void TestConnection(string host, string username, string password)
        {
            try
            {
                using var conn = new NpgsqlConnection($"Host={host};Username={username};Password={password};Database=inventory");
                conn.Open();

                using var cmd = new NpgsqlCommand("SELECT version()", conn);
                string version = cmd.ExecuteScalar()?.ToString();

                Console.WriteLine($"PostgreSQL version: {version}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                throw new Exception("failed to connect to database", ex);
            }
        }
    }
}
