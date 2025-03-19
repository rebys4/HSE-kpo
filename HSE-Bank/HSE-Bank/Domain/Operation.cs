namespace HSE_Bank.Domain;

public enum OperationType {Income, Expense}

public class Operation
{
    public Guid Id { get;  }
    public OperationType Type { get;  }
    public Guid BankAccountId { get;  }
    public decimal Amount { get;  }
    public DateTime Date { get;  }
    public string Description { get;  }
    public Guid CategoryId { get;  }

    public Operation(Guid id, OperationType type, Guid bankAccountId, decimal amount, DateTime date, Guid categoryId, string description = null)
    {
        if (amount < 0) { throw new ArgumentException("Сумма не может быть отрицательной", nameof(amount)); }

        Id = id;
        Type = type;
        BankAccountId = bankAccountId;
        Amount = amount;
        Date = date;
        Description = description;
        CategoryId = categoryId;
    }
}