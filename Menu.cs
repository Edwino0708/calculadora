namespace Calculator;

/// <summary>
/// A menu option: its name, its symbol and the method that calculates it.
/// </summary>
public record MenuOption(string Name, string Symbol, Func<double, double, double> Calculate);

public static class Menu
{
    // To add a new operation: create the method in Operations.cs
    // and add a line here.
    public static readonly Dictionary<string, MenuOption> Options = new()
    {
        ["1"] = new("Add", "+", Operations.Add),
        ["2"] = new("Subtract", "-", Operations.Subtract),
        ["3"] = new("Multiply", "*", Operations.Multiply),
        ["4"] = new("Divide", "/", Operations.Divide),
    };

    public static void Show()
    {
        Console.WriteLine();
        Console.WriteLine("===== CALCULATOR =====");
        foreach (var (key, option) in Options)
            Console.WriteLine($"{key}. {option.Name}");
        Console.WriteLine("H. History");
        Console.WriteLine("0. Exit");
    }
}
