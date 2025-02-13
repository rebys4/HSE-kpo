using System;
using MoscowZooERP.Interfaces;
using MoscowZooERP.Animals;

namespace MoscowZooERP.Services;

public class VeterinaryClinic : IVeterinaryClinic
{
    public bool CheckAnimal(Animal animal)
    {
        Console.WriteLine($"Ветеринарная клиника: проведите осмотр животного \"{animal.Name}\" (без инвентарного номера пока).");
        Console.WriteLine("Введите состояние здоровья животного (\"здоров\" или \"не здоров\"):");
        string input = Console.ReadLine().Trim().ToLower();

        return input == "здоров" || input == "здоровый" || input == "да" || input == "yes";
    }
}