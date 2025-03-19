using System.Diagnostics;
using HSE_Bank.Commands;
namespace HSE_Bank.Commands.Decorators;

public class CommandTimeDecorator : ICommand
{
    private readonly ICommand _command;

    public CommandTimeDecorator(ICommand command)
    {
        _command = command;
    }

    public void Execute()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        _command.Execute();
        stopwatch.Stop();
        Console.WriteLine($"Время выполнения команды: {stopwatch.ElapsedMilliseconds} мс");
    }
}