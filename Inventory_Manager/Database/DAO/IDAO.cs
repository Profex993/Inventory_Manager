using Inventory_Manager.Entities;

namespace Inventory_Manager.Database.DAO
{
    public interface IDAO
    {
        List<Entity> GetAll();
        bool Edit(Entity edited);
        bool Delete(Entity delete);
        bool Add(Entity add);

    }
}
