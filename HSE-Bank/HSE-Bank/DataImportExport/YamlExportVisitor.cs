using HSE_Bank.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HSE_Bank.DataImportExport;

public class YamlExportVisitor : IDataExportVisitor
{
    private readonly List<object> _yamlObjects = new List<object>();
    public string FilePath { get;  }

    public YamlExportVisitor(string filePath)
    {
        FilePath = filePath;
    }
    public void ExportBankAccount(BankAccount account)
    {
        _yamlObjects.Add(account);
    }
    
    public void ExportCategory(Category category)
    {
        _yamlObjects.Add(category);
    }
    
    public void ExportOperation(Operation operation)
    {
        _yamlObjects.Add(operation);
    }

    public void SaveToFile()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        string yaml = serializer.Serialize(_yamlObjects);
        File.WriteAllText(FilePath, yaml);
    }
}