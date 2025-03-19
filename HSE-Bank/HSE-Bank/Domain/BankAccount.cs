namespace HSE_Bank.Domain;

public class BankAccount
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }


    public BankAccount(Guid id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    public void UpadteBalance(decimal amount)
    {
        Balance += amount;
    }

    public void SetName(string name)
    {
        Name = name;
    }
}