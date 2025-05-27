using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;

namespace Inventory_Manager.Database
{
    class TableManager
    {
        private static TableManager? _instance;
        public enum TableNames
        {
            Parts,
            PartLog,
            Devices,
            DeviceLog,
            Bom
        }

        private readonly Dictionary<TableNames, Type> daos = new();
        private readonly Dictionary<TableNames, Type> entityTypes = new();

        public static TableManager Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private TableManager()
        {
            daos.Add(TableNames.Parts, typeof(PartsDAO));
            daos.Add(TableNames.PartLog, typeof(PartLogDao));
            daos.Add(TableNames.Devices, typeof(DeviceDAO));
            daos.Add(TableNames.DeviceLog, typeof(DeviceLogDao));
            daos.Add(TableNames.Bom, typeof(BomDAO));

            entityTypes.Add(TableNames.Parts, typeof(Part));
            entityTypes.Add(TableNames.PartLog, typeof(PartLog));
            entityTypes.Add(TableNames.Devices, typeof(Device));
            entityTypes.Add(TableNames.DeviceLog, typeof(DeviceLog));
            entityTypes.Add(TableNames.Bom, typeof(Bom));
        }

        public IDAO GetDAO(string tableName)
        {
            return (IDAO)Activator.CreateInstance(daos[Enum.Parse<TableNames>(tableName)]);
        }

        public Type GetEntityType(string tableName)
        {
            return entityTypes[Enum.Parse<TableNames>(tableName)];
        }

        public List<string> GetTableNames()
        {
            return [.. Enum.GetNames<TableNames>()];
        }
    }
}
