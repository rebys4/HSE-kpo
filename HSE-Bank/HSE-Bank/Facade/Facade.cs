using HSE_Bank.Domain;
using HSE_Bank.Factories;
using HSE_Bank.Repositories;

namespace HSE_Bank.Facades;

public class Facade
{
    private readonly IDataRepository _dataRepository;

    public Facade(IDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }

    public BankAccount CreateBankAccount(string name, decimal balance = 0)
    {
        var account = DomainFactory.CreateBankAccount(name, balance);
        _dataRepository.AddBankAccount(account);
        return account;
    }

    public void UpdateAccountName(Guid id, string newName)
    {
        var account = _dataRepository.GetBankAccount(id);
        if (account != null)
        {
            account.SetName(newName);
            _dataRepository.UpdateBankAccount(account);
        }
    }

    public void DeleteBankAccount(Guid id)
    {
        _dataRepository.DeleteBankAccount(id);
    }

    public Category CreateCategory(CategoryType type, string name)
    {
        var category = DomainFactory.CreateCategory(type, name);
        _dataRepository.AddCategory(category);
        return category;
    }

    public void UpdateCategoryName(Guid id, string newName)
    {
        var category = _dataRepository.GetCategory(id);
        if (category != null)
        {
            category.SetName(newName);
            _dataRepository.UpdateCategory(category);
        }
    }

    public void DeleteCategory(Guid id)
    {
        _dataRepository.DeleteCategory(id);
    }

    public Operation CreateOperation(OperationType type, Guid accountId, decimal amount, 
        DateTime date, Guid categoryId, string description = null)
    {
        var operation = DomainFactory.CreateOperation(type, accountId, amount, date, categoryId, description);
        _dataRepository.AddOperation(operation);
        var account = _dataRepository.GetBankAccount(accountId);
        if (account != null)
        {
            var delta = type == OperationType.Income ? amount : -amount;
            account.UpadteBalance(delta);
            _dataRepository.UpdateBankAccount(account);
        }
        return operation;
    }

    public void DeleteOperation(Guid id)
    {
        _dataRepository.DeleteOperation(id);
    }

    public decimal CalculateNewBalance(DateTime start, DateTime end)
    {
        var operations = _dataRepository.GetOperations()
            .Where(op => op.Date.Date >= start && op.Date.Date <= end);
        decimal net = 0;
        foreach (var operation in operations)
        {
            net += operation.Type == OperationType.Income ? operation.Amount : -operation.Amount;
        }
        return net;
    }

    public IDictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end)
    {
        var operations = _dataRepository.GetOperations()
            .Where(op => op.Date >= start && op.Date <= end);
        
        return operations.GroupBy(op => op.CategoryId).
            ToDictionary(
                g => g.Key, 
                g => g.Sum(op => op.Type == OperationType.Income ? op.Amount : -op.Amount));
    }
}