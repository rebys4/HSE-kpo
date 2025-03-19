using System;
using System.Collections.Generic;
using HSE_Bank.Domain;

namespace HSE_Bank.Repositories;

public interface IDataRepository
{
    void AddBankAccount(BankAccount bankAccount);
    BankAccount GetBankAccount(Guid id);
    void UpdateBankAccount(BankAccount bankAccount);
    void DeleteBankAccount(Guid id);
    
    void AddCategory(Category category);
    Category GetCategory(Guid id);
    void UpdateCategory(Category category);
    void DeleteCategory(Guid id);
    
    void AddOperation(Operation operation);
    IEnumerable<Operation> GetOperations();
    void DeleteOperation(Guid id);
}