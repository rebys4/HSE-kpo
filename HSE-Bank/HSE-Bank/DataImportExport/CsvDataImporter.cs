namespace HSE_Bank.DataImportExport;

public class CsvDataImporter : DataImporter
{
    protected override void Parse(string data)
    {
        try
        {
            var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
            {
                Console.WriteLine("CSV файл пуст или не содержит данных.");
                return;
            }
            var headers = lines[0].Split(',');
            
            Console.WriteLine($"Импортировано {lines.Length - 1} строк из CSV файла.");
            foreach (var line in lines.Skip(1))
            {
                var fields = line.Split(',');
                Console.WriteLine($"Строка: {string.Join(" | ", fields)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при импорте CSV: {ex.Message}");
        }
    }
}