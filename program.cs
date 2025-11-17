using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("CmdCalc by Fredrik Norling, XPageDeveloper 2025");
            Console.WriteLine("Examples");
            Console.WriteLine("CmdCalc 10+6");
            Console.WriteLine("CmdCalc 20% of 100");
            Console.WriteLine("CmdCalc 200 - 10%");
            Console.WriteLine("CmdCalc ((10*3)-3)/3");
            Console.WriteLine("CmdCalc 2x+4=10");
            Console.WriteLine("CmdCalc 3x-9=0");
            return;
        }

        var input = string.Concat(args).Trim();

        if (input.Contains("x"))
        {
            var solved = SolveLinear(input);
            Console.WriteLine(solved);
            return;
        }

        var step1 = ExpandOf(input);
        var step2 = ExpandPercentAuto(step1);
        var normalized = NormalizePercent(step2);
        var result = Evaluate(normalized);
        Console.WriteLine(result.ToString(CultureInfo.InvariantCulture));
    }

    static string SolveLinear(string expr)
    {
        var pattern = @"([\+\-]?\d*\.?\d*)x([\+\-]?\d*\.?\d*)=([\+\-]?\d*\.?\d*)";
        var m = Regex.Match(expr, pattern);
        if (!m.Success) return "Unsupported";

        var aStr = m.Groups[1].Value;
        var bStr = m.Groups[2].Value;
        var cStr = m.Groups[3].Value;

        double a = aStr == "" || aStr == "+" ? 1 : aStr == "-" ? -1 : double.Parse(aStr, CultureInfo.InvariantCulture);
        double b = bStr == "" ? 0 : double.Parse(bStr, CultureInfo.InvariantCulture);
        double c = double.Parse(cStr, CultureInfo.InvariantCulture);

        var x = (c - b) / a;
        return x.ToString(CultureInfo.InvariantCulture);
    }

    static string ExpandOf(string expr)
    {
        var pattern = @"(\d+(\.\d+)?)%\s*of|av\s*(\d+(\.\d+)?)";
        return Regex.Replace(expr, pattern, m =>
        {
            var percent = m.Groups[1].Value;
            var value = m.Groups[3].Value;
            return "(" + percent + "%*" + value + ")";
        }, RegexOptions.IgnoreCase);
    }

    static string ExpandPercentAuto(string expr)
    {
        var pattern = @"(\d+(\.\d+)?)\s*([\+\-\*/])\s*(\d+(\.\d+)?%)";
        return Regex.Replace(expr, pattern, m =>
        {
            var baseNum = m.Groups[1].Value;
            var op = m.Groups[3].Value;
            var percent = m.Groups[4].Value;
            return baseNum + op + "(" + baseNum + "*" + percent + ")";
        });
    }

    static string NormalizePercent(string expr)
    {
        var pattern = @"(\d+(\.\d+)?)%";
        return Regex.Replace(expr, pattern, m =>
        {
            var num = double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
            return "(" + (num / 100.0).ToString(CultureInfo.InvariantCulture) + ")";
        });
    }

    static double Evaluate(string expression)
    {
        var dt = new DataTable();
        dt.CaseSensitive = false;
        var value = dt.Compute(expression, "");
        return Convert.ToDouble(value, CultureInfo.InvariantCulture);
    }
}
