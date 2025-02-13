using MoscowZooERP.Interfaces;
using MoscowZooERP.Utils;

namespace MoscowZooERP.Animals
{
    public abstract class Animal : IInventory, IAlive
    {
        public string Name { get; set; }
        public int Food { get; set; }
        public int Number { get; set; }

        public Animal(string name, int food)
        {
            Number = InventoryNumberGenerator.GetNextNumber();
            Name = name;
            Food = food;
        }

        public override string ToString()
        {
            return $"{GetType().Name} - {Name}, Инвентарный номер: {Number}, Еды: {Food} кг/сутки";
        }
    }
}