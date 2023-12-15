using System.Text;

namespace AdventOfCode;

public class Task15_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var line = lines.ToList()[0];
        var result = ProcessLine(line);
        
        Console.WriteLine(result);
    }

    static int ProcessLine(string line)
    {
        var sequences = line.Split(",");
        var sum = 0;
        foreach (var sequence in sequences)
        {
            sum += GetHash(sequence);
        }

        return sum;
    }

    static int GetHash(string s)
    {
        var asciiValues =  Encoding.ASCII.GetBytes(s);
        var currentValue = 0;
        for (int i = 0; i < s.Length; i++)
        {
            currentValue = ((currentValue + asciiValues[i]) * 17) % 256;
        }

        return currentValue;
    }
}