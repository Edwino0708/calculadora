# Console Calculator

A simple C# (.NET 10) project for practicing Git.

## How to run it

```bash
dotnet run
```

```
===== CALCULATOR =====
1. Add
2. Subtract
3. Multiply
4. Divide
H. History
0. Exit
Choose an option: 1
First number: 5
Second number: 3
Result: 5 + 3 = 8
```

## Structure

| File                | Contents                                        |
|---------------------|-------------------------------------------------|
| `Program.cs`        | Entry point: starts the app                     |
| `CalculatorApp.cs`  | Main loop: shows the menu and runs the choice   |
| `Menu.cs`           | List of options and how the menu is printed     |
| `Operations.cs`     | Math methods (Add, Subtract, Multiply, Divide)  |
| `InputReader.cs`    | Reads text and numbers from the keyboard        |
| `History.cs`        | Stores the calculations made in the session     |
| `Calculator.csproj` | Project configuration                           |
| `.gitignore`        | Keeps `bin/` and `obj/` out of the repo         |

### Adding a new operation

1. Add the method in `Operations.cs`:
   ```csharp
   public static double Power(double a, double b) => Math.Pow(a, b);
   ```
2. Add one line in `Menu.cs`:
   ```csharp
   ["5"] = new("Power", "^", Operations.Power),
   ```

## Git practice

See **[GIT-PRACTICE.md](GIT-PRACTICE.md)** for step-by-step exercises:

1. Two branches change **different files** → automatic merge
2. Two branches change the **same file, different lines** → automatic merge
3. Two branches change the **same line** → conflict
4. Two branches change **both files** → conflict in both, and how to resolve it
