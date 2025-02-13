using System;
using Microsoft.Extensions.DependencyInjection;
using MoscowZooERP.Animals;
using MoscowZooERP.Interfaces;
using MoscowZooERP.Inventory;
using MoscowZooERP.Services;
using MoscowZooERP.Domain;

namespace ZooERP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка DI-контейнера
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Zoo>()                              // Один экземпляр зоопарка на всё приложение
                .AddSingleton<IVeterinaryClinic, VeterinaryClinic>() // Регистрация ветеринарной клиники
                .BuildServiceProvider();

            // Получаем нужные сервисы
            var zoo = serviceProvider.GetService<Zoo>();
            var vetClinic = serviceProvider.GetService<IVeterinaryClinic>();

            // Добавим в зоопарк несколько инвентаризационных вещей для демонстрации
            zoo.AddThing(new Table("Стол для посетителей"));
            zoo.AddThing(new Computer("Компьютер учета"));

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить новое животное");
                Console.WriteLine("2. Вывести отчет по животным и потреблению еды");
                Console.WriteLine("3. Вывести список животных для контактного зоопарка");
                Console.WriteLine("4. Вывести список инвентаризационных предметов");
                Console.WriteLine("5. Выход");

                string choice = Console.ReadLine().Trim();
                switch (choice)
                {
                    case "1":
                        AddNewAnimal(zoo, vetClinic);
                        break;
                    case "2":
                        ReportFoodConsumption(zoo);
                        break;
                    case "3":
                        ReportContactAnimals(zoo);
                        break;
                    case "4":
                        ReportInventoryItems(zoo);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }

        /// <summary>
        /// Метод добавления нового животного.
        /// Перед добавлением вызывается ветеринарная клиника для проверки состояния здоровья.
        /// </summary>
        private static void AddNewAnimal(Zoo zoo, IVeterinaryClinic vetClinic)
        {
            Console.WriteLine("Введите тип животного (Monkey, Rabbit, Tiger, Wolf):");
            string type = Console.ReadLine().Trim().ToLower();

            Console.WriteLine("Введите наименование животного:");
            string name = Console.ReadLine().Trim();

            Console.WriteLine("Введите количество килограммов еды в сутки:");
            int food;
            while (!int.TryParse(Console.ReadLine(), out food))
            {
                Console.WriteLine("Введите корректное число для количества еды:");
            }

            Animal animal = null;
            switch (type)
            {
                case "monkey":
                    Console.WriteLine("Введите уровень доброты (от 0 до 10):");
                    int kindness;
                    while (!int.TryParse(Console.ReadLine(), out kindness))
                    {
                        Console.WriteLine("Введите корректное число для уровня доброты:");
                    }
                    animal = new Monkey(name, food, kindness);
                    break;
                case "rabbit":
                    Console.WriteLine("Введите уровень доброты (от 0 до 10):");
                    while (!int.TryParse(Console.ReadLine(), out kindness))
                    {
                        Console.WriteLine("Введите корректное число для уровня доброты:");
                    }
                    animal = new Rabbit(name, food, kindness);
                    break;
                case "tiger":
                    animal = new Tiger(name, food);
                    break;
                case "wolf":
                    animal = new Wolf(name, food);
                    break;
                default:
                    Console.WriteLine("Неверный тип животного.");
                    return;
            }

            // Проверка состояния животного через ветеринарную клинику
            bool isHealthy = vetClinic.CheckAnimal(animal);
            if (isHealthy)
            {
                zoo.AddAnimal(animal);
                Console.WriteLine($"Животное \"{animal.Name}\" успешно принято в зоопарк.");
            }
            else
            {
                Console.WriteLine($"Животное \"{animal.Name}\" не прошло осмотр и не принято в зоопарк.");
            }
        }

        /// <summary>
        /// Метод формирования отчёта по количеству животных и суммарному потреблению еды.
        /// </summary>
        private static void ReportFoodConsumption(Zoo zoo)
        {
            int totalFood = zoo.GetTotalFoodConsumption();
            int count = zoo.AnimalCount();
            Console.WriteLine($"Всего животных: {count}");
            Console.WriteLine($"Общее количество потребляемой еды в день: {totalFood} кг");
        }

        /// <summary>
        /// Метод формирования списка животных, подходящих для контактного зоопарка.
        /// </summary>
        private static void ReportContactAnimals(Zoo zoo)
        {
            var contactAnimals = zoo.GetContactAnimals();
            Console.WriteLine("Животные, подходящие для контактного зоопарка:");
            foreach (var animal in contactAnimals)
            {
                Console.WriteLine($"- {animal.Name} (Inv #{animal.Number}), Уровень доброты: {animal.Kindness}");
            }
        }

        /// <summary>
        /// Метод вывода списка всех инвентаризационных предметов (животных и вещей).
        /// </summary>
        private static void ReportInventoryItems(Zoo zoo)
        {
            Console.WriteLine("Инвентаризационные предметы (животные и вещи):");
            foreach (var item in zoo.GetAllInventoryItems())
            {
                if (item is Animal animal)
                {
                    Console.WriteLine($"Животное: {animal.Name} (Inv #{animal.Number})");
                }
                else if (item is Thing thing)
                {
                    Console.WriteLine($"Вещь: {thing.Name} (Inv #{thing.Number})");
                }
            }
        }
    }
}
