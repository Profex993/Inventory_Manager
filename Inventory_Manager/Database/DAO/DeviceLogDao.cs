using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    class DeviceLogDao : IDAO
    {
        public List<Entity> GetAll()
        {
            var deviceLogs = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM device_logs", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                deviceLogs.Add(new DeviceLog
                {
                    Id = reader.GetInt32(0),
                    DeviceId = reader.GetInt32(1),
                    Quantity = reader.GetInt32(2),
                    BuildBy = reader.GetString(3),
                    Date = reader.GetDateTime(4)
                });
            }

            return deviceLogs;
        }

        public void Edit(Entity edited)
        {
            throw new NotImplementedException();
        }

        public void Delete(Entity delete)
        {
            throw new NotImplementedException();
        }
    }
}
