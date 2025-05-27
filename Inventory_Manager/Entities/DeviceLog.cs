namespace Inventory_Manager.Entities
{
    class DeviceLog : Entity
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public int DeviceId { get; set; }
        public int Quantity { get; set; }
        public string BuildBy { get; set; }
        public DateTime Date { get; set; }
    }
}
