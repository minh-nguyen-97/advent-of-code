namespace AdventOfCode;

public class Task13_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        long sum = 0;
        var grid = new List<string>();
        var patterns = lines.ToList();
        patterns.Add(string.Empty);
        foreach (var line in patterns)
        {
            if (string.IsNullOrEmpty(line))
            {
                sum += ProcessGrid(grid);
                grid = new List<string>();
            }
            else
            {
                grid.Add(line);
            }
        }
        
        Console.WriteLine(sum);
    }

    static int ProcessGrid(List<string> grid)
    {
        var a = grid.ToArray();
        var horizontalReflection = FindReflection(a);
        if (horizontalReflection != -1) return horizontalReflection * 100;
        
        var b = Rotate(a);
        var verticalReflection = FindReflection(b);
        return verticalReflection;
    }

    static string[] Rotate(string[] a)
    {
        var b = new string[a[0].Length];
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < a[0].Length; j++)
            {
                b[j] += a[i][j];
            }
        }

        return b;
    }

    static bool CanFindSmudge(string a, string b)
    {
        var numOfDiff = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                numOfDiff++;
                if (numOfDiff > 1) return false;
            }
        }

        return numOfDiff == 1;
    }

    static int FindReflection(string[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            var leftReflection = i;
            var rightReflection = i + 1;
            var foundReflection = true;
            var hasSmudge = false;
            while (leftReflection >= 0 && rightReflection < a.Length)
            {
                if (a[leftReflection] != a[rightReflection])
                {
                    if (CanFindSmudge(a[leftReflection], a[rightReflection]) && !hasSmudge)
                    {
                        hasSmudge = true;
                    }
                    else
                    {
                        foundReflection = false;
                        break;
                    }
                }

                leftReflection--;
                rightReflection++;
            }

            if (foundReflection)
            {
                return i + 1;
            }
        }

        return -1;
    }
}