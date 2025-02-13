namespace MoscowZooERP.Utils;

public static class InventoryNumberGenerator
{
    private static int _currentNumber = 1;
    public static int GetNextNumber() => _currentNumber++;
}