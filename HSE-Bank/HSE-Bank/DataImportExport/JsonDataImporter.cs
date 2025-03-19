using HSE_Bank.Domain;
using Newtonsoft.Json;

namespace HSE_Bank.DataImportExport;

public class JsonDataImporter : DataImporter
{
    protected override void Parse(string data)
    {
        try
        {
            var jsondata = JsonConvert.DeserializeObject<List<BankAccount>>(data);
            Console.WriteLine("Импортированы данные из JSON файла");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при парсинге данных: {ex.Message}");
        }
    }
}