using HSE_Bank.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HSE_Bank.DataImportExport;

public class JsonExportVisitor : IDataExportVisitor
{
    private readonly List<object> _jsonObj = new List<object>();
    public string FilePath { get; }

    public JsonExportVisitor(string filePath)
    {
        FilePath = filePath;
    }
    public void ExportBankAccount(BankAccount bankAccount)
    { 
        var json = JObject.FromObject(bankAccount);
        _jsonObj.Add(json);
    }

    public void ExportCategory(Category category)
    {
        var json = JObject.FromObject(category);
        _jsonObj.Add(json);
    }

    public void ExportOperation(Operation operation)
    {
        var json = JObject.FromObject(operation);
        _jsonObj.Add(json);
    }

    public void SaveToFile()
    {
        var array = new JArray(_jsonObj);
        string finalJson = array.ToString(Formatting.Indented);
        File.WriteAllText(FilePath, finalJson);
    }
    
}