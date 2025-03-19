namespace HSE_Bank.Domain;
public enum CategoryType {Income, Expense}

public class Category
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public CategoryType Type { get;  }

    public Category(Guid id, string name, CategoryType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public void SetName(string name)
    {
        Name = name;
    }
}