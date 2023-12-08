namespace AdventOfCode;

public class Task1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../task1.txt");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }
}