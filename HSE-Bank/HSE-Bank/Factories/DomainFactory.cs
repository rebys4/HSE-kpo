using System;
using HSE_Bank.Domain;

namespace HSE_Bank.Factories;

public static class DomainFactory
{
    public static BankAccount CreateBankAccount(string name, decimal balance = 0)
    {
        return new BankAccount(Guid.NewGuid(), name, balance);
    }

    public static Category CreateCategory(CategoryType type, string name)
    {
        return new Category(Guid.NewGuid(), name, type);
    }

    public static Operation CreateOperation(OperationType type, Guid bankAccountId, decimal amount, DateTime date, Guid categoryId, string description = null)
    {
        return new Operation(Guid.NewGuid(), type, bankAccountId, amount, date, categoryId, description);
    }
}