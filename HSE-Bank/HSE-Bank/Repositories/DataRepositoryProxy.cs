using System;
using System.Collections.Generic;
using HSE_Bank.Domain;

namespace HSE_Bank.Repositories;

public class DataRepositoryProxy : IDataRepository
{
    public Dictionary<Guid, BankAccount> Accounts { get; } = new Dictionary<Guid, BankAccount>();
    public Dictionary<Guid, Category> Categories { get; } = new Dictionary<Guid, Category>();
    public Dictionary<Guid, Operation> Operations { get; } = new Dictionary<Guid, Operation>();

    public void AddBankAccount(BankAccount account) => Accounts[account.Id] = account;

    public BankAccount GetBankAccount(Guid accountId)
    {
        Accounts.TryGetValue(accountId, out var account);
        return account;
    }

    public void UpdateBankAccount(BankAccount account) => Accounts[account.Id] = account;
    public void DeleteBankAccount(Guid id) => Accounts.Remove(id); 
    public void AddCategory(Category category) => Categories[category.Id] = category;

    public Category GetCategory(Guid categoryId)
    {
        Categories.TryGetValue(categoryId, out var category);
        return category;
    }

    public void UpdateCategory(Category category) => Categories[category.Id] = category;
    public void DeleteCategory(Guid id) => Categories.Remove(id);
    public void AddOperation(Operation operation) => Operations[operation.Id] = operation;

    public IEnumerable<Operation> GetOperations()
    {
        return Operations.Values;
    }
    
    public void DeleteOperation(Guid id) => Operations.Remove(id);
}