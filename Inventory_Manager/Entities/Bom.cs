//short for bill of material
namespace Inventory_Manager.Entities
{
    class Bom : Entity
    {
        [Id]
        public int Id { get; set; }
        [Id]
        public int DeviceId { get; set; }
        [Id]
        public int PartId { get; set; }
        public string DeviceName { get; set; }
        public string PartName { get; set; }
        public int RequiredAmount { get; set; }
    }
}
