namespace Calculator;

/// <summary>
/// Keeps the list of calculations made during the session.
/// </summary>
public class History
{
    private readonly List<string> entries = new();

    public void Add(string calculation)
    {
        entries.Add(calculation);
    }

    public void Show()
    {
        Console.WriteLine();
        Console.WriteLine("===== HISTORY =====");

        if (entries.Count == 0)
        {
            Console.WriteLine("No calculations yet.");
            return;
        }

        for (int i = 0; i < entries.Count; i++)
            Console.WriteLine($"{i + 1}. {entries[i]}");
    }
}
