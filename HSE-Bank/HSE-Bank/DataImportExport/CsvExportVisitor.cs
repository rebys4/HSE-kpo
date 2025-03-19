using System.Text;
using HSE_Bank.Domain;

namespace HSE_Bank.DataImportExport;

public class CsvExportVisitor : IDataExportVisitor
{
    private readonly StringBuilder _sb = new StringBuilder();
    public string FilePath { get; }

    public CsvExportVisitor(string filePath)
    {
        FilePath = filePath;
        _sb.AppendLine("Type,Id,Name,Balance,OperationType,BankAccountId,Amount,Date,CategoryId,Description");
    }
    public void ExportBankAccount(BankAccount account)
    {
        string line = $"BankAccount,{account.Id},{account.Name},{account.Balance},,,,,";
        _sb.AppendLine(line);
    }
    
    public void ExportCategory(Category category)
    {
        string line = $"Category,{category.Id},{category.Type},{category.Name},,,,,";
        _sb.AppendLine(line);
    }
    
    public void ExportOperation(Operation operation)
    {
        string line = $"Operation,{operation.Id},,{(int)0},{operation.BankAccountId},{operation.Amount},{operation.Date},{operation.CategoryId},{operation.Description}";
        _sb.AppendLine(line);
    }

    public void SaveToFile()
    {
        File.WriteAllText(FilePath, _sb.ToString());
    }
}