namespace AdventOfCode;

public class Task3_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../example.txt");

        var sum = 0;
        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }

        Console.WriteLine(sum);
    }

    class NumberInLine
    {
        public int StartPos;
        public int EndPos;
        public int Value;
    }

    static int ProcessLine(string line)
    {
        return 0;
    }
}