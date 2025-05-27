namespace Inventory_Manager.Entities
{
    class Part : Entity
    {
        [Id]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
