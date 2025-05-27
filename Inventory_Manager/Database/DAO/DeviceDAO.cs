using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    class DeviceDAO : IDAO
    {
        public List<Entity> GetAll()
        {
            var devices = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM devices", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                devices.Add(new Device
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return devices;
        }

        public int GetIdByName(string name)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT id FROM devices WHERE name = @name;", conn);
            cmd.Parameters.AddWithValue("@name", name);
            conn.Open();

            var result = cmd.ExecuteScalar();

            if (result == null)
            {
                throw new Exception("Device name not found.");
            }

            return (int)result;
        }

        public void Edit(Entity edited)
        {
            Device device = edited as Device;
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "UPDATE devices " +
                "SET name = @name " +
                "WHERE id = @originalId;",
            conn);

            cmd.Parameters.AddWithValue("@name", device.Name);
            cmd.Parameters.AddWithValue("@originalId", device.Id);

            conn.Open();

            cmd.ExecuteNonQuery();
        }

        public void Delete(Entity delete)
        {
            Device device = delete as Device;
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM devices WHERE id = @originalId;",
            conn);

            cmd.Parameters.AddWithValue("@originalId", device.Id);

            conn.Open();

            cmd.ExecuteNonQuery();
        }
    }
}
