using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    internal class BomDAO : IDAO
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

            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    boms.Add(new Bom
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        DeviceId = reader.GetInt32(reader.GetOrdinal("device_id")),
                        PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                        RequiredAmount = reader.GetInt32(reader.GetOrdinal("quantity_required")),
                        DeviceName = reader.GetString(reader.GetOrdinal("device_name")),
                        PartName = reader.GetString(reader.GetOrdinal("part_name"))
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve BOM entries", ex);
            }

            return boms;
        }

        public bool Edit(Entity edited)
        {
            if (edited is not Bom bom) throw new InvalidCastException("Provided entity is not a BOM");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "UPDATE boms " +
                "SET device_id = @device_id, " +
                "part_id = @part_id, " +
                "quantity_required = @quantity_required " +
                "WHERE id = @originalId;",
            conn);

            try
            {
                cmd.Parameters.AddWithValue("@device_id", bom.DeviceId);
                cmd.Parameters.AddWithValue("@part_id", bom.PartId);
                cmd.Parameters.AddWithValue("@quantity_required", bom.RequiredAmount);
                cmd.Parameters.AddWithValue("@originalId", bom.Id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to edit BOM entry", ex);
            }
        }

        public bool Delete(Entity delete)
        {
            if (delete is not Bom bom) throw new InvalidCastException("Provided entity is not a BOM");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM boms WHERE id = @originalId;",
            conn);

            try
            {
                cmd.Parameters.AddWithValue("@originalId", bom.Id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete BOM entry", ex);
            }
        }

        public bool Add(Entity add)
        {
            if (add is not Bom bom) throw new InvalidCastException("Provided entity is not a BOM");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "INSERT INTO boms (device_id, part_id, quantity_required) " +
                "VALUES (@device_id, @part_id, @quantity_required);",
            conn);

            try
            {
                cmd.Parameters.AddWithValue("@device_id", bom.DeviceId);
                cmd.Parameters.AddWithValue("@part_id", bom.PartId);
                cmd.Parameters.AddWithValue("@quantity_required", bom.RequiredAmount);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add BOM entry", ex);
            }
        }

        public void AddAll(IEnumerable<Entity> entities)
        {
            List<Bom> boms = entities as List<Bom>;
            using var conn = DatabaseConnection.Instance.GetConnection();

            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                foreach (Bom bom in boms)
                {
                    using var cmd = new NpgsqlCommand(
                                        "INSERT INTO boms (device_id, part_id, quantity_required) " +
                                        "VALUES (@device_id, @part_id, @quantity_required);",
                                    conn, tran);
                    cmd.Parameters.AddWithValue("@device_id", bom.DeviceId);
                    cmd.Parameters.AddWithValue("@part_id", bom.PartId);
                    cmd.Parameters.AddWithValue("@quantity_required", bom.RequiredAmount);
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception("Failed to add BOM entries", ex);
            }
        }
    }
}
