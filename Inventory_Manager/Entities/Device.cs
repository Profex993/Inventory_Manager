using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Manager.Entities
{
    class Device : Entity
    {
        [Id]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
