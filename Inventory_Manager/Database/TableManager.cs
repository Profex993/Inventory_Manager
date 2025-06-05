using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;

namespace Inventory_Manager.Database
{
    //singleton for managing tables
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

        private readonly Dictionary<TableNames, Type> daos = [];
        private readonly Dictionary<TableNames, Type> entityTypes = [];

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

        /// <summary>
        /// get dao for given table
        /// </summary>
        /// <param name="tableName">string name of table</param>
        /// <returns>IDAO instance</returns>
        public IDAO GetDAO(string tableName)
        {
            return (IDAO)Activator.CreateInstance(daos[Enum.Parse<TableNames>(tableName)]);
        }

        /// <summary>
        /// get typeof entity for a table
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <returns>typeof entity tied to a table</returns>
        public Type GetEntityType(string tableName)
        {
            return entityTypes[Enum.Parse<TableNames>(tableName)];
        }

        /// <summary>
        /// get all table names
        /// </summary>
        /// <returns>List of string names</returns>
        public List<string> GetTableNames()
        {
            return [.. Enum.GetNames<TableNames>()];
        }
    }
}
