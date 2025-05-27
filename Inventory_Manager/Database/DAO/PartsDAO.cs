using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    internal class PartsDAO : IDAO
    {
        public List<Entity> GetAll()
        {
            var parts = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM parts", conn);

            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    parts.Add(new Part
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("quantity"))
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve parts", ex);
            }

            return parts;
        }

        public int GetIdByName(string name)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT id FROM parts WHERE name = @name;", conn);
            cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Text).Value = name;

            try
            {
                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result == null) throw new Exception($"Part with name '{name}' not found.");

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get part ID by name", ex);
            }
        }

        public bool Edit(Entity edited)
        {
            if (edited is not Part part) throw new InvalidCastException("Provided entity is not a Part.");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "UPDATE parts SET name = @name, quantity = @quantity WHERE id = @originalId;",
                conn);

            try
            {
                cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Text).Value = part.Name;
                cmd.Parameters.Add("@quantity", NpgsqlTypes.NpgsqlDbType.Integer).Value = part.Quantity;
                cmd.Parameters.Add("@originalId", NpgsqlTypes.NpgsqlDbType.Integer).Value = part.Id;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update part", ex);
            }
        }

        public bool Delete(Entity delete)
        {
            if (delete is not Part part) throw new InvalidCastException("Provided entity is not a Part.");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("DELETE FROM parts WHERE id = @originalId;", conn);

            try
            {
                cmd.Parameters.Add("@originalId", NpgsqlTypes.NpgsqlDbType.Integer).Value = part.Id;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete part", ex);
            }
        }

        public bool Add(Entity add)
        {
            if (add is not Part part) throw new InvalidCastException("Provided entity is not a Part.");

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("INSERT INTO parts (name, quantity) VALUES (@name, @quantity);", conn);

            try
            {
                cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Text).Value = part.Name;
                cmd.Parameters.Add("@quantity", NpgsqlTypes.NpgsqlDbType.Integer).Value = part.Quantity;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add part", ex);
            }
        }

        public List<string> GetAllNames()
        {
            List<string> list = [];
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT name FROM parts", conn);

            try
            {
                conn.Open();

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(reader.GetOrdinal("name")));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get part names", ex);
            }

            return list;
        }
    }
}
