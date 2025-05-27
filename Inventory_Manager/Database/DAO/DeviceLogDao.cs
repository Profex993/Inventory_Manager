using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    class DeviceLogDao : IDAO
    {
        public bool Add(Entity add)
        {
            throw new NotImplementedException("Logs can not be edited");
        }

        public bool Delete(Entity delete)
        {
            throw new NotImplementedException("Logs can not be edited");
        }

        public bool Edit(Entity edited)
        {
            throw new NotImplementedException("Logs can not be edited");
        }

        public List<Entity> GetAll()
        {
            var deviceLogs = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM device_logs", conn);

            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    deviceLogs.Add(new DeviceLog
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        DeviceId = reader.GetInt32(reader.GetOrdinal("device_id")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("quantity_built")),
                        BuildBy = reader.GetString(reader.GetOrdinal("built_by")),
                        Date = reader.GetDateTime(reader.GetOrdinal("timestamp"))
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve device logs", ex);
            }

            return deviceLogs;
        }
    }
}
