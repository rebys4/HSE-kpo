using HSE_Bank.Domain;

namespace HSE_Bank.DataImportExport;

public interface IDataExportVisitor
{
    void ExportBankAccount(BankAccount bankAccount);
    void ExportCategory(Category category);
    void ExportOperation(Domain.Operation operation);
}