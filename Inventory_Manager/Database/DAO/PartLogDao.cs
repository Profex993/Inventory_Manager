using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    class PartLogDao : IDAO
    {
        public List<Entity> GetAll()
        {
            var partLogs = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM part_logs", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                partLogs.Add(new PartLog
                {
                    Id = reader.GetInt32(0),
                    PartId = reader.GetInt32(1),
                    QuantityChanged = reader.GetInt32(2),
                    ChangedBy = reader.GetString(3),
                    Note = reader.GetString(4),
                    Date = reader.GetDateTime(5)
                });
            }

            return partLogs;
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
