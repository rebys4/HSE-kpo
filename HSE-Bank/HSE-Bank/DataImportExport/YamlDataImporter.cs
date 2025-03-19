using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HSE_Bank.DataImportExport;

public class YamlDataImporter : DataImporter
{
    protected override void Parse(string rawData)
    {
        try
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var data = deserializer.Deserialize<object>(rawData);
            Console.WriteLine("Импортированы данные из YAML файла!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при импорте YAML: {e.Message}");
        }
    }
}