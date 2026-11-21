# Git Practice Guide

Hands-on exercises using this calculator to understand **branches, merges and conflicts**.

## The key idea

> Git merges **line by line**, not file by file.

When two branches change the code, Git only cares about **which lines** each branch touched:

| Branch A changes            | Branch B changes            | Result                              |
|-----------------------------|-----------------------------|-------------------------------------|
| `History.cs`                | `Operations.cs`             | ✅ Automatic merge                  |
| `Menu.cs`, line 23          | `Menu.cs`, line 27          | ✅ Automatic merge                  |
| `Menu.cs`, line 23          | `Menu.cs`, line 23          | ❌ **Conflict** in `Menu.cs`        |
| Both files, **same lines**  | Both files, **same lines**  | ❌ **Conflict** in **both** files   |
| Both files, different lines | Both files, different lines | ✅ Automatic merge                  |

The exercises below prove each row of this table.

---

## Setup

```bash
git init -b main
git add .
git commit -m "Basic calculator"
git status          # bin/ and obj/ do NOT appear thanks to .gitignore
```

> **Tip:** when a merge creates a new commit, Git opens an editor for the message.
> Just save and close it (in Vim: type `:wq` and press Enter).

---

## Exercise 1 — Different files ✅ no conflict

**Branch A** changes `History.cs`:

```bash
git switch -c change-history-title
```
In `History.cs` change `"===== HISTORY ====="` to `"===== YOUR CALCULATIONS ====="`.
```bash
git commit -am "Change history title"
```

**Branch B** changes `Operations.cs`:

```bash
git switch main
git switch -c change-divide-message
```
In `Operations.cs` change `"Cannot divide by zero"` to `"Division by zero is not allowed"`.
```bash
git commit -am "Change divide by zero message"
```

**Merge both:**

```bash
git switch main
git merge change-history-title
git merge change-divide-message
```

**Result:** both merges succeed. Each branch touched a different file, so there is nothing to decide.

---

## Exercise 2 — Same file, different lines ✅ no conflict

Both branches edit `Menu.cs`, but in **different lines**.

```bash
git switch -c change-menu-title
```
In `Menu.cs` change `"===== CALCULATOR ====="` to `"===== GIT CALCULATOR ====="`.
```bash
git commit -am "Change menu title"

git switch main
git switch -c change-exit-text
```
In `Menu.cs` change `"0. Exit"` to `"0. Quit"`.
```bash
git commit -am "Change exit text"

git switch main
git merge change-menu-title
git merge change-exit-text
```

**Result:** automatic merge. Same file, but the lines don't overlap, so Git keeps both changes.
Run `dotnet run` to see the new title **and** "Quit".

---

## Exercise 3 — Same line ❌ conflict

Both branches change **the same line** (the menu title) in different ways.

```bash
git switch -c title-english
```
In `Menu.cs` change the title to `"===== MY CALCULATOR ====="`.
```bash
git commit -am "Title in English"

git switch main
git switch -c title-spanish
```
In `Menu.cs` change the title to `"===== CALCULADORA ====="`.
```bash
git commit -am "Title in Spanish"

git switch main
git merge title-english
git merge title-spanish
```

**Result:**

```
Auto-merging Menu.cs
CONFLICT (content): Merge conflict in Menu.cs
Automatic merge failed; fix conflicts and then commit the result.
```

Open `Menu.cs`:

```csharp
<<<<<<< HEAD
        Console.WriteLine("===== MY CALCULATOR =====");
=======
        Console.WriteLine("===== CALCULADORA =====");
>>>>>>> title-spanish
```

Git **can't decide** which title is correct, so a human must choose.
→ See [How to resolve a conflict](#how-to-resolve-a-conflict).

---

## Exercise 4 — Both files changed in both branches ❌ conflict in both

Two developers add a new operation at the same time. Each one touches
**`Operations.cs` and `Menu.cs`**, and both use option `"5"`.

**Developer A — Power:**

```bash
git switch -c feature/power
```
`Operations.cs`, after the `Divide` method:
```csharp
    public static double Power(double a, double b) => Math.Pow(a, b);
```
`Menu.cs`, after the `["4"]` line:
```csharp
        ["5"] = new("Power", "^", Operations.Power),
```
```bash
git commit -am "Add power operation"
```

**Developer B — Modulo:**

```bash
git switch main
git switch -c feature/modulo
```
`Operations.cs`, after the `Divide` method:
```csharp
    public static double Modulo(double a, double b) => a % b;
```
`Menu.cs`, after the `["4"]` line:
```csharp
        ["5"] = new("Modulo", "%", Operations.Modulo),
```
```bash
git commit -am "Add modulo operation"
```

**Merge both:**

```bash
git switch main
git merge feature/power
git merge feature/modulo
```

**Result:**

```
Auto-merging Menu.cs
CONFLICT (content): Merge conflict in Menu.cs
Auto-merging Operations.cs
CONFLICT (content): Merge conflict in Operations.cs
Automatic merge failed; fix conflicts and then commit the result.
```

`git status --short` shows both files as `UU` (unmerged):

```
UU Menu.cs
UU Operations.cs
```

**Resolve it:** here we want **both** features, so keep both lines in each file:

`Operations.cs` → keep both methods:
```csharp
    public static double Power(double a, double b) => Math.Pow(a, b);

    public static double Modulo(double a, double b) => a % b;
```

`Menu.cs` → keep both options and change Modulo to `"6"`:
```csharp
        ["5"] = new("Power", "^", Operations.Power),
        ["6"] = new("Modulo", "%", Operations.Modulo),
```

```bash
dotnet run                 # check that it works: 2 ^ 10 = 1024, 10 % 3 = 1
git add Operations.cs Menu.cs
git commit -m "Merge feature/modulo: keep power and modulo"
git log --oneline --graph --all
```

> 💡 **Lesson:** a conflict is not an error. It's Git asking a question it can't answer alone.
> Sometimes the answer is "keep A", sometimes "keep B", and sometimes, like here, **"keep both"**.

---

## How to resolve a conflict

1. `git status` → shows which files are in conflict.
2. Open each file and find the markers:
   ```
   <<<<<<< HEAD          ← what is in your current branch (main)
   ...
   =======
   ...
   >>>>>>> other-branch  ← what comes from the branch you are merging
   ```
3. Edit the code to leave it as it should be and **delete the three marker lines**.
   (VS Code shows buttons: *Accept Current*, *Accept Incoming*, *Accept Both*.)
4. `dotnet run` → make sure it still compiles and works.
5. `git add <file>` for each resolved file.
6. `git commit` to finish the merge.

Changed your mind? `git merge --abort` cancels the merge and goes back to how it was before.

---

## Useful commands

| Command                             | What it does                                   |
|-------------------------------------|------------------------------------------------|
| `git status`                        | Changed files, staged files, conflicts         |
| `git diff`                          | Line-by-line changes not yet staged            |
| `git log --oneline --graph --all`   | History of all branches as a graph             |
| `git branch`                        | Lists branches (`*` = current)                 |
| `git switch -c <name>`              | Creates a branch and switches to it            |
| `git merge <branch>`                | Brings the other branch's changes into this one |
| `git merge --abort`                 | Cancels a merge in progress                    |
| `git branch -d <name>`              | Deletes a branch that was already merged       |
| `git blame Menu.cs`                 | Who changed each line, and in which commit     |
| `git stash` / `git stash pop`       | Saves unfinished changes aside and restores them |
| `git revert <commit>`               | Creates a new commit that undoes another one   |
