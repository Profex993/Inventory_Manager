using Inventory_Manager.Entities;
using Npgsql;

namespace Inventory_Manager.Database.DAO
{
    class PartsDAO : IDAO
    {
        public List<Entity> GetAll()
        {
            var parts = new List<Entity>();

            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM parts", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                parts.Add(new Part
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Quantity = reader.GetInt32(2)
                });

            }

            return parts;
        }

        public int GetIdByName(string name)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT id FROM parts WHERE name = @name;", conn);
            cmd.Parameters.AddWithValue("@name", name);
            conn.Open();

            var result = cmd.ExecuteScalar();

            if (result == null)
            {
                throw new Exception("Part name not found.");
            }

            return (int)result;
        }

        public bool AddPart(Part part)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand("INSERT INTO parts (name, quantity) VALUES (@name, @quantity)", conn);
            
            cmd.Parameters.AddWithValue("name", part.Name);
            cmd.Parameters.AddWithValue("quantity", part.Quantity);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public void Edit(Entity edited)
        {
            Part part = edited as Part;
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "UPDATE parts " +
                "SET name = @name, " +
                "quantity = @quantity " +
                "WHERE id = @originalId;",
            conn);

            cmd.Parameters.AddWithValue("@name", part.Name);
            cmd.Parameters.AddWithValue("@quantity", part.Quantity);
            cmd.Parameters.AddWithValue("@originalId", part.Id);

            conn.Open();

            cmd.ExecuteNonQuery();
        }

        public void Delete(Entity delete)
        {
            Part part = delete as Part;
            using var conn = DatabaseConnection.Instance.GetConnection();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM parts WHERE id = @originalId;",
            conn);

            cmd.Parameters.AddWithValue("@originalId", part.Id);

            conn.Open();

            cmd.ExecuteNonQuery();
        }
    }
}
