namespace MoscowZooERP.Animals;

public abstract class Herbo : Animal
{
    public int Kindness { get; set; }

    protected Herbo(string name, int food, int kidness) : base(name, food)
    {
        Kindness = kidness;
    }
    
    public bool CanContact() =>  Kindness > 5;

    public override string ToString() => base.ToString() + $", Доброта: {Kindness}";
}