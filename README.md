# CmdCalc

CmdCalc is a small command-line calculator written in C# that targets the .NET Framework 4.8. It evaluates arithmetic expressions, understands percentages and "of" syntax, and can solve simple linear equations in the form ax+b=c.

**Features:**
- **Basic arithmetic:** addition, subtraction, multiplication, division and parentheses.
- **Percent handling:** expressions like `20% of 100`, `200 - 10%`, and `50% * 3` are supported and normalized automatically.
- **Linear equation solving:** solves simple one-variable linear equations such as `2x+4=10` or `3x-9=0`.
- **Single-file C# project:** `Program.cs` contains the full implementation.

**How it works (brief):**
- The program concatenates CLI arguments and inspects the input.
- If the expression contains `x`, it attempts to solve a linear equation using a regular expression. It computes `x = (c - b) / a` for expressions matching `ax+b=c`.
- Percent expressions are expanded so that `N%` becomes `(N/100)` and patterns like `A - B%` are interpreted as `A - (A * (B/100))` when appropriate.
- Final numeric evaluation uses `System.Data.DataTable.Compute()` to evaluate the normalized arithmetic expression.

Requirements
- Windows: .NET Framework 4.8 (Visual Studio or Developer Command Prompt)
- Linux/macOS: Mono (for running the compiled .NET Framework executable)

Build & run

Windows (Visual Studio / Developer Command Prompt):

1. Open a "Developer Command Prompt for VS".
2. Compile:

```
csc -out:CmdCalc.exe Program.cs
```

3. Run:

```
CmdCalc.exe 10+6
```

Linux/macOS with Mono:

1. Install Mono (if not installed): see `https://www.mono-project.com/`
2. Compile with `mcs` (Mono C# compiler) or reuse the Windows-built `CmdCalc.exe`:

```
mcs -out:CmdCalc.exe Program.cs
mono CmdCalc.exe 20% of 100
```

Examples and expected output

- `CmdCalc 10+6`
	- Output: `16`
- `CmdCalc "20% of 100"`
	- Output: `20` (expands `20% of 100` → `(20%*100)` → `0.2*100`)
- `CmdCalc 200 - 10%`
	- Output: `180` (interpreted as `200 - (200*10%)` → `200 - 20`)
- `CmdCalc ((10*3)-3)/3`
	- Output: `9`
- `CmdCalc 2x+4=10`
	- Output: `3` (solves for `x`)
- `CmdCalc 3x-9=0`
	- Output: `3` (solves for `x`)

Implementation notes
- Linear equations are matched with a regex: `([\+\-]?\d*\.?\d*)x([\+\-]?\d*\.?\d*)=([\+\-]?\d*\.?\d*)`.
- Percent normalization converts `N%` into `(N/100)` using invariant culture to ensure consistent decimal parsing.
- Expression evaluation uses `DataTable.Compute` — this keeps the expression parsing simple but means the evaluator is tied to the behaviors of `Compute`.

Contributing
- Feel free to open issues or pull requests. Small, focused changes are easiest to review.

License
- Apache 2.0 license

Author
- CmdCalc by Fredrik Norling, XPageDeveloper (2025)
