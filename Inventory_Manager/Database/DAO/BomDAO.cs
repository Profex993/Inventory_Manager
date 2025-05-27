using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    class BomDAO : IDAO
    {
        public List<Entity> GetAll()
        {
            var boms = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "SELECT b.id, b.device_id, b.part_id, b.quantity_required, d.name AS device_name, p.name AS part_name " +
                "FROM boms b " +
                "INNER JOIN devices d ON b.device_id = d.id " +
                "INNER JOIN parts p ON b.part_id = p.id;",
            conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                boms.Add(new Bom
                {
                    Id = reader.GetInt32(0),
                    DeviceId = reader.GetInt32(1),
                    PartId = reader.GetInt32(2),
                    RequiredAmount = reader.GetInt32(3),
                    DeviceName = reader.GetString(4),
                    PartName = reader.GetString(5)
                });
            }

            return boms;
        }

        public void Edit(Entity edited)
        {
            Bom bom = edited as Bom;
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "UPDATE boms " +
                "SET device_id = @device_id, " +
                "part_id = @part_id, " +
                "quantity_required = @quantity_required " +
                "WHERE id = @originalId;",
            conn);

            cmd.Parameters.AddWithValue("@device_id", bom.DeviceId);
            cmd.Parameters.AddWithValue("@part_id", bom.PartId);
            cmd.Parameters.AddWithValue("@quantity_required", bom.RequiredAmount);
            cmd.Parameters.AddWithValue("@originalId", bom.Id);

            conn.Open();

            cmd.ExecuteNonQuery();
        }

        public void Delete(Entity delete)
        {
            Bom bom = delete as Bom;
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM boms WHERE id = @originalId;",
            conn);

            cmd.Parameters.AddWithValue("@originalId", bom.Id);

            conn.Open();

            cmd.ExecuteNonQuery();
        }
    }
}
