namespace AdventOfCode;

public class Task12_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var sum = 0;
        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }
        
        Console.WriteLine(sum);
    }

    static bool IsPossibleContinuous(Char c)
    {
        return c == '#' || c == '?';
    }
    
    
    static bool IsPossibleBlock(Char c)
    {
        return c == '.' || c == '?';
    }

    static bool IsRealPossible(Char c)
    {
        return c == '?';
    }


    static int ProcessLine(string line)
    {
        var parts = line.Split(" ");
        var a = "." + parts[0];
        var b = parts[1].Split(",").Select(x => Int32.Parse(x)).ToArray();

        var possibleContinuous = new bool[a.Length, a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            if (IsPossibleContinuous(a[i]))
            {
                possibleContinuous[i, i] = true;
                if (i > 0)
                {
                    for (int j = i-1; j >= 0; j--)
                    {
                        if (IsPossibleContinuous(a[j]))
                        {
                            possibleContinuous[j, i] = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        var F = new int[a.Length, b.Length];

        for (int i = 1; i < a.Length; i++)
        {
            if (a[i] == '.' || a[i] == '?')
            {
                F[i, 0] = F[i - 1, 0];
            }

            if (i >= b[0] && possibleContinuous[i - b[0] + 1, i] )
            {
                var continuousWays = 0;
                if (i == b[0]) continuousWays = 1;
                if (i > b[0]) continuousWays = F[i - b[0] - 1, 0] + 1;
                if (a[i] == '#') F[i, 0] = continuousWays;
                if (a[i] == '?') F[i, 0] += continuousWays;
            }
        }

        for (int j = 1; j < b.Length; j++)
        {
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] == '.' || a[i] == '?') F[i, j] = F[i - 1, j];
                if (i - b[j] > 0 && IsPossibleBlock(a[i - b[j]]) && possibleContinuous[i - b[j] + 1, i])
                {
                    if (a[i] == '#') F[i, j] = F[i - b[j] - 1, j - 1];
                    if (a[i] == '?') F[i, j] += F[i - b[j] - 1, j - 1];
                }

            }
        }

        return F[a.Length - 1, b.Length - 1];
    }
}