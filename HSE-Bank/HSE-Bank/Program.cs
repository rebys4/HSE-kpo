using System;
using Microsoft.Extensions.DependencyInjection;
using HSE_Bank.Domain;
using HSE_Bank.Facades;
using HSE_Bank.Repositories;
using HSE_Bank.Commands;
using HSE_Bank.Commands.Decorators;
using HSE_Bank.DataImportExport;

namespace HSE_Bank
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Настройка DI-контейнера
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Получаем необходимые сервисы через DI
            var facade = serviceProvider.GetService<Facade>();
            var repository = serviceProvider.GetService<IDataRepository>();

            RunInteractiveMenu(facade, repository);

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        private static void RunInteractiveMenu(Facade financeFacade, IDataRepository repository)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Меню модуля 'Учет финансов' ===");
                Console.WriteLine("1. Создать счет");
                Console.WriteLine("2. Создать категорию");
                Console.WriteLine("3. Создать операцию");
                Console.WriteLine("4. Показать все счета");
                Console.WriteLine("5. Показать все категории");
                Console.WriteLine("6. Показать все операции");
                Console.WriteLine("7. Вычислить чистую сумму (доходы - расходы)");
                Console.WriteLine("8. Экспорт данных в JSON, CSV и YAML");
                Console.WriteLine("9. Выход");
                Console.Write("Выберите пункт меню: ");
                
                string input = Console.ReadLine();
                Console.WriteLine();
                switch (input)
                {
                    case "1":
                        CreateAccount(financeFacade);
                        break;
                    case "2":
                        CreateCategory(financeFacade);
                        break;
                    case "3":
                        CreateOperation(financeFacade, repository);
                        break;
                    case "4":
                        ShowAllAccounts(repository);
                        break;
                    case "5":
                        ShowAllCategories(repository);
                        break;
                    case "6":
                        ShowAllOperations(repository);
                        break;
                    case "7":
                        CalculateNetAmount(financeFacade);
                        break;
                    case "8":
                        ExportData(serviceProvider: null, repository);
                        break;
                    case "9":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private static void CreateAccount(Facade facade)
        {
            Console.Write("Введите название счета: ");
            string name = Console.ReadLine();
            Console.Write("Введите начальный баланс: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal balance))
            {
                var account = facade.CreateBankAccount(name, balance);
                Console.WriteLine($"Создан счет: {account.Name} с балансом {account.Balance}, Id: {account.Id}");
            }
            else
            {
                Console.WriteLine("Неверный формат баланса.");
            }
        }

        private static void CreateCategory(Facade facade)
        {
            Console.Write("Введите название категории: ");
            string name = Console.ReadLine();
            Console.Write("Введите тип категории (1 - Доход, 2 - Расход): ");
            string typeInput = Console.ReadLine();
            if (typeInput == "1")
            {
                var category = facade.CreateCategory(CategoryType.Income, name);
                Console.WriteLine($"Создана категория: {category.Name}, Тип: {category.Type}, Id: {category.Id}");
            }
            else if (typeInput == "2")
            {
                var category = facade.CreateCategory(CategoryType.Expense, name);
                Console.WriteLine($"Создана категория: {category.Name}, Тип: {category.Type}, Id: {category.Id}");
            }
            else
            {
                Console.WriteLine("Неверный тип категории.");
            }
        }

        private static void CreateOperation(Facade facade, IDataRepository repository)
        {
            Console.Write("Введите Id счета: ");
            string accountIdInput = Console.ReadLine();
            if (!Guid.TryParse(accountIdInput, out Guid accountId))
            {
                Console.WriteLine("Неверный Id счета.");
                return;
            }

            Console.Write("Введите сумму операции: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Неверный формат суммы.");
                return;
            }

            Console.Write("Введите описание операции (необязательно): ");
            string description = Console.ReadLine();

            Console.Write("Введите Id категории: ");
            string categoryIdInput = Console.ReadLine();
            if (!Guid.TryParse(categoryIdInput, out Guid categoryId))
            {
                Console.WriteLine("Неверный Id категории.");
                return;
            }

            Console.Write("Введите тип операции (1 - Доход, 2 - Расход): ");
            string opTypeInput = Console.ReadLine();
            OperationType opType;
            if (opTypeInput == "1")
            {
                opType = OperationType.Income;
            }
            else if (opTypeInput == "2")
            {
                opType = OperationType.Expense;
            }
            else
            {
                Console.WriteLine("Неверный тип операции.");
                return;
            }

            try
            {
                var op = facade.CreateOperation(opType, accountId, amount, DateTime.Now, categoryId, description);
                Console.WriteLine($"Создана операция с Id: {op.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании операции: {ex.Message}");
            }
        }

        private static void ShowAllAccounts(IDataRepository repository)
        {
            Console.WriteLine("Счета:");
            foreach (var acc in ((Repositories.DataRepositoryProxy)repository).Accounts.Values)
            {
                Console.WriteLine($"{acc.Id} - {acc.Name}, Баланс: {acc.Balance}");
            }
        }

        private static void ShowAllCategories(IDataRepository repository)
        {
            Console.WriteLine("Категории:");
            foreach (var cat in ((Repositories.DataRepositoryProxy)repository).Categories.Values)
            {
                Console.WriteLine($"{cat.Id} - {cat.Name}, Тип: {cat.Type}");
            }
        }

        private static void ShowAllOperations(IDataRepository repository)
        {
            Console.WriteLine("Операции:");
            foreach (var op in repository.GetOperations())
            {
                Console.WriteLine($"{op.Id} - {op.Description}, Сумма: {op.Amount}, Дата: {op.Date}");
            }
        }

        private static void CalculateNetAmount(Facade facade)
        {
            Console.Write("Введите начало периода (yyyy-MM-dd): ");
            string startInput = Console.ReadLine();
            Console.Write("Введите конец периода (yyyy-MM-dd): ");
            string endInput = Console.ReadLine();

            if (DateTime.TryParse(startInput, out DateTime start) && DateTime.TryParse(endInput, out DateTime end))
            {
                decimal net = facade.CalculateNewBalance(start, end);
                Console.WriteLine($"Чистая сумма за период: {net}");
            }
            else
            {
                Console.WriteLine("Неверный формат даты.");
            }
        }

        private static void ExportData(IServiceProvider serviceProvider, IDataRepository repository)
        {
            // Пример экспорта. Здесь можно получить экспортёров через DI.
            // Для демонстрации мы просто создадим экспортеры с фиксированными именами файлов.
            var jsonExporter = new JsonExportVisitor("export.json");
            var csvExporter = new CsvExportVisitor("export.csv");
            var yamlExporter = new YamlExportVisitor("export.yaml");

            foreach (var acc in ((Repositories.DataRepositoryProxy)repository).Accounts.Values)
            {
                jsonExporter.ExportBankAccount(acc);
                csvExporter.ExportBankAccount(acc);
                yamlExporter.ExportBankAccount(acc);
            }
            foreach (var cat in ((Repositories.DataRepositoryProxy)repository).Categories.Values)
            {
                jsonExporter.ExportCategory(cat);
                csvExporter.ExportCategory(cat);
                yamlExporter.ExportCategory(cat);
            }
            foreach (var op in repository.GetOperations())
            {
                jsonExporter.ExportOperation(op);
                csvExporter.ExportOperation(op);
                yamlExporter.ExportOperation(op);
            }

            jsonExporter.SaveToFile();
            csvExporter.SaveToFile();
            yamlExporter.SaveToFile();

            Console.WriteLine("Данные экспортированы в файлы export.json, export.csv и export.yaml");
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Регистрация зависимостей
            services.AddSingleton<IDataRepository, Repositories.DataRepositoryProxy>();
            services.AddTransient<Facade>();
            
            services.AddTransient<JsonExportVisitor>(provider => new JsonExportVisitor("export.json"));
            services.AddTransient<CsvExportVisitor>(provider => new CsvExportVisitor("export.csv"));
            services.AddTransient<YamlExportVisitor>(provider => new YamlExportVisitor("export.yaml"));
        }
    }
}
