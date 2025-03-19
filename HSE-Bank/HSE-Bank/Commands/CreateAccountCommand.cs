using System;
using HSE_Bank.Facades;

namespace HSE_Bank.Commands;

public class CreateAccountCommand : ICommand
{ 
    private readonly Facade _facade;
    private readonly string _name;
    private readonly decimal _balance;

    public CreateAccountCommand(Facade facade, string name, decimal balance)
    {
        _facade = facade;
        _name = name;
        _balance = balance;
    }

    public void Execute()
    {
        var account = _facade.CreateBankAccount(_name, _balance);
        Console.WriteLine($"Создан счет: {account.Name} (Баланс: {account.Balance})");
    }
}