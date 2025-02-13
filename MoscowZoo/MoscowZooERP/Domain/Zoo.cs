using System.Collections.Generic;
using System.Linq;
using MoscowZooERP.Animals;
using MoscowZooERP.Inventory;
using MoscowZooERP.Interfaces;

namespace MoscowZooERP.Domain
{
    /// <summary>
    /// Класс, описывающий зоопарк.
    /// Содержит списки животных и инвентаризационных вещей.
    /// </summary>
    public class Zoo
    {
        private List<Animal> _animals = new List<Animal>();
        private List<Thing> _things = new List<Thing>();

        public IReadOnlyList<Animal> Animals => _animals;
        public IReadOnlyList<Thing> Things => _things;

        /// <summary>
        /// Добавляет животное в зоопарк. Инвентарный номер уже установлен в конструкторе.
        /// </summary>
        public void AddAnimal(Animal animal)
        {
            _animals.Add(animal);
        }

        /// <summary>
        /// Добавляет вещь в зоопарк. Инвентарный номер уже установлен в конструкторе.
        /// </summary>
        public void AddThing(Thing thing)
        {
            _things.Add(thing);
        }

        /// <summary>
        /// Возвращает суммарное количество килограммов еды, потребляемых животными за сутки.
        /// </summary>
        public int GetTotalFoodConsumption()
        {
            return _animals.Sum(a => a.Food);
        }

        /// <summary>
        /// Возвращает список животных (только травоядных), пригодных для контактного зоопарка.
        /// </summary>
        public IEnumerable<Herbo> GetContactAnimals()
        {
            return _animals.OfType<Herbo>().Where(a => a.CanContact());
        }

        /// <summary>
        /// Возвращает все объекты, подлежащие инвентаризации (животные и вещи).
        /// </summary>
        public IEnumerable<IInventory> GetAllInventoryItems()
        {
            var items = new List<IInventory>();
            items.AddRange(_animals);
            items.AddRange(_things);
            return items;
        }

        /// <summary>
        /// Возвращает количество животных в зоопарке.
        /// </summary>
        public int AnimalCount()
        {
            return _animals.Count;
        }
    }
}
