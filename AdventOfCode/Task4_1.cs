namespace AdventOfCode;

public class Task4_1
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
        var numbersParts = line.Split(": ")[1].Split(" | ");
        var winningNumbers = numbersParts[0].Split(" ")
            .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
            .Select(x => Int32.Parse(x.Trim()))
            .ToHashSet();
        var owningNumbers = numbersParts[1].Split(" ")
            .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
            .Select(x => Int32.Parse(x.Trim()))
            .ToArray();

        var numOfOwningWinning = 0;
        foreach (var owningNumber in owningNumbers)
        {
            if (winningNumbers.Contains(owningNumber))
            {
                numOfOwningWinning++;
            }
        }
        
        var winningPoint = 0;
        if (numOfOwningWinning > 0)
        {
            winningPoint = 1;
            for (int i = 1; i < numOfOwningWinning; i++)
            {
                winningPoint *= 2;
            }
        }
        
        return winningPoint;
    }
}