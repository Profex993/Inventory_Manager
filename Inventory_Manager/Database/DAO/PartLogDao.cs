using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    class PartLogDao : IDAO
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
            var partLogs = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM part_logs", conn);

            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    partLogs.Add(new PartLog
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        PartId = reader.IsDBNull(reader.GetOrdinal("part_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("part_id")),
                        PartName = reader.GetString(reader.GetOrdinal("part_name")),
                        QuantityChanged = reader.GetInt32(reader.GetOrdinal("quantity_changed")),
                        ChangedBy = reader.GetString(reader.GetOrdinal("changed_by")),
                        Note = reader.GetString(reader.GetOrdinal("note")),
                        Date = reader.GetDateTime(reader.GetOrdinal("timestamp"))
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve part logs", ex);
            }

            return partLogs;
        }

    }
}
