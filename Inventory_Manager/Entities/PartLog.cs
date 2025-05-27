namespace Inventory_Manager.Entities
{
    class PartLog : Entity
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public int QuantityChanged { get; set; }
        public string ChangedBy { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}
