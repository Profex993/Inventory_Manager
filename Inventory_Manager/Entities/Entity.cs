//class representing a row in each table
namespace Inventory_Manager.Entities
{
    public abstract class Entity
    {

    }

    //to define id attributes which can not be directly edited
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IdAttribute : Attribute
    {
    }
}
