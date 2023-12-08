namespace AdventOfCode;

public class Task1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../task1.txt");

        var sum = 0;
        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }
        
        Console.WriteLine(sum);
    }

    public static int ProcessLine(string line)
    {
        var firstDigit = -1;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) && line[i] != '0')
            {
                firstDigit = line[i] - '0';
                break;
            }
        }

        if (firstDigit < 0)
        {
            return 0;
        }

        var lastDigit = -1;
        for (int i = line.Length - 1; i >= 0; i--)
        {
            if (Char.IsDigit(line[i]))
            {
                lastDigit = line[i] - '0';
                break;
            }
        }

        if (lastDigit < 0) return 0;

        return firstDigit * 10 + lastDigit;
    }

}