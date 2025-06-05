using Npgsql;

//singleton class for storing connection string and getting DatabaseConnection objects
namespace Inventory_Manager.Database
{
    public class DatabaseConnection
    {

        private static readonly Lazy<DatabaseConnection> _instance = new(() => new DatabaseConnection());

        private static string _host;
        private static string _username;
        private static string _password;

        private static string connectionString;

        private DatabaseConnection()
        {
            connectionString = $"Host={_host};Username={_username};Password={_password};Database=inventory";
        }

        /// <summary>
        /// method which initializes the connection string in the singleton DatabaseConnection
        /// needs to be called before using the database
        /// </summary>
        /// <param name="host">hostname of the database server</param> 
        /// <param name="username">username of database user</param> 
        /// <param name="password">password of database user</param> 
        /// <exception cref="InvalidOperationException">when database was initialized previously</exception> 
        public static void Initialize(string host, string username, string password)
        {
            if (_instance.IsValueCreated)
                throw new InvalidOperationException("DatabaseConnection is already initialized.");

            _host = host;
            _username = username;
            _password = password;
        }

        public static DatabaseConnection Instance => _instance.Value;

        /// <summary>
        /// get object of database connection
        /// </summary>
        /// <returns>Npgsqlconnection object</returns>
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// test database connection with given connection credentials
        /// </summary>
        /// <param name="host">hostname of the database server</param> 
        /// <param name="username">username of database user</param> 
        /// <param name="password">password of database user</param> 
        /// <exception cref="Exception">throws exception when can not connect to database server</exception>
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
