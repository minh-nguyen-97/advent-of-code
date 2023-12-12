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

    static int ProcessLine(string line)
    {
        return 0;
    }
}