﻿namespace Inventory_Manager.Entities
{
    class Device : Entity
    {
        [Id]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
