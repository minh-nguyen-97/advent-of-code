namespace AdventOfCode;

public class Task12_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        long sum = 0;
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

    private static int unfoldTimes = 5;
    
    static long ProcessLine(string line)
    {
        var parts = line.Split(" ");
        
        var firstPart = parts[0];
        for (int i = 0; i < unfoldTimes - 1; i++)
        {
            firstPart += "?" + parts[0];
        }
        var a = "#." + firstPart;
        
        var secondPart = parts[1].Split(",").Select(x => Int32.Parse(x)).ToList();
        var bList = new List<int>(secondPart);
        for (int i = 0; i < unfoldTimes - 1; i++)
        {
            bList.AddRange(new List<int>(secondPart));
        }
        bList.Insert(0, 1);
        var b = bList.ToArray();

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

        var F = new long[a.Length, b.Length, 2];
        F[0, 0, 1] = 1;
        F[1, 0, 0] = 1;

        for (int j = 0; j < b.Length; j++)
        {
            for (int i = 2; i < a.Length; i++)
            {
                if (a[i] == '.' || a[i] == '?')
                {
                    F[i, j, 0] = F[i - 1, j, 0] + F[i - 1, j, 1];
                }

                if (a[i] == '#' || a[i] == '?')
                {
                    if (i > b[j] && j > 0 && possibleContinuous[i - b[j] + 1, i])
                        F[i, j, 1] = F[i - b[j], j - 1, 0];
                }
            }
        }

        long result = 0;

        if (a[a.Length - 1] == '.') result = F[a.Length - 1, b.Length - 1, 0];
        if (a[a.Length - 1] == '#') result = F[a.Length - 1, b.Length - 1, 1];
        if (a[a.Length - 1] == '?') result = F[a.Length - 1, b.Length - 1, 0] + F[a.Length - 1, b.Length - 1, 1];

        return result;
    }
}