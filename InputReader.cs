using System.Globalization;

namespace Calculator;

/// <summary>
/// Reads text and numbers typed by the user.
/// </summary>
public static class InputReader
{
    public static string ReadText(string message)
    {
        Console.Write(message);
        string? text = Console.ReadLine();

        // End of input (Ctrl+Z): exit cleanly
        if (text is null)
        {
            Console.WriteLine();
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }

        return text.Trim();
    }

    public static double ReadNumber(string message)
    {
        while (true)
        {
            // Accepts both "2.5" and "2,5"
            string text = ReadText(message).Replace(',', '.');

            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
                return number;

            Console.WriteLine("That is not a valid number, try again.");
        }
    }
}
