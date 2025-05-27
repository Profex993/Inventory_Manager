using Inventory_Manager.Entities;

namespace Inventory_Manager.Database.DAO
{
    public  interface IDAO
    {
        internal List<Entity> GetAll();
        internal void Edit(Entity edited);
        internal void Delete(Entity delete);
    }
}
