namespace Calculator;

/// <summary>
/// Main loop: shows the menu, reads the user's choice and runs it.
/// </summary>

///Probando.
/////probando algo nuevo.
public class CalculatorApp
{
    private readonly History history = new();

    public void Run()
    {
        while (true)
        {
            Menu.Show();
            string choice = InputReader.ReadText("Choose an option: ").ToUpper();

            if (choice == "0")
            {
                Console.WriteLine("Goodbye!");
                return;
            }

            if (choice == "H")
            {
                history.Show();
                continue;
            }

            if (Menu.Options.TryGetValue(choice, out var option))
                Calculate(option);
            else
                Console.WriteLine("Invalid option.");
        }
    }

    private void Calculate(MenuOption option)
    {
        double a = InputReader.ReadNumber("First number: ");
        double b = InputReader.ReadNumber("Second number: ");

        try
        {
            double result = option.Calculate(a, b);
            string calculation = $"{a} {option.Symbol} {b} = {result}";

            Console.WriteLine($"Result: {calculation}");
            history.Add(calculation);
        }
        catch (ArithmeticException error)
        {
            Console.WriteLine($"Error: {error.Message}");
        }
    }
}
