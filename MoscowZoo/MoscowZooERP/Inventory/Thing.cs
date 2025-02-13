using MoscowZooERP.Interfaces;
using MoscowZooERP.Utils;

namespace MoscowZooERP.Inventory;

public abstract class Thing : IInventory
{
    public string Name { get; set; }
    public int Number { get; set; }

    public Thing(string name)
    {
        Number = InventoryNumberGenerator.GetNextNumber();
        Name = name;
    }

    public override string ToString()
    {
        return $"{GetType().Name} - {Name}, Инвентарный номер - {Number}";
    }
}