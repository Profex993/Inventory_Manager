using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    internal class DeviceDAO : IDAO
    {
        public List<Entity> GetAll()
        {
            var devices = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM devices", conn);

            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    devices.Add(new Device
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name"))
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve devices", ex);
            }

            return devices;
        }

        public int GetIdByName(string name)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT id FROM devices WHERE name = @name;", conn);
            cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Text).Value = name;

            try
            {
                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result == null) throw new Exception($"Device with name '{name}' not found.");

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get device ID by name", ex);
            }
        }

        public bool Edit(Entity edited)
        {
            if (edited is not Device device) throw new InvalidCastException("Provided entity is not a Device.");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "UPDATE devices SET name = @name WHERE id = @originalId;",
                conn);

            try
            {
                cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Text).Value = device.Name;
                cmd.Parameters.Add("@originalId", NpgsqlTypes.NpgsqlDbType.Integer).Value = device.Id;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to edit device", ex);
            }
        }

        public bool Delete(Entity delete)
        {
            if (delete is not Device device) throw new InvalidCastException("Provided entity is not a Device.");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM devices WHERE id = @originalId;",
                conn);

            try
            {
                cmd.Parameters.Add("@originalId", NpgsqlTypes.NpgsqlDbType.Integer).Value = device.Id;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete device", ex);
            }
        }

        public bool Add(Entity add)
        {
            if (add is not Device device) throw new InvalidCastException("Provided entity is not a Device.");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "INSERT INTO devices (name) VALUES (@name);",
                conn);

            try
            {
                cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Text).Value = device.Name;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add device", ex);
            }
        }
    }
}
