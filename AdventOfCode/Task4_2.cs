namespace AdventOfCode;

public class Task4_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");
        var linesArr = lines.ToArray();

        var sum = 0;
        var aggSum = new int[linesArr.Length + 1];
        for (int i = 0; i < linesArr.Length; i++)
        {
            var numOfMatches = ProcessLine(linesArr[i]);
            var endIndex = Math.Min(i + numOfMatches, linesArr.Length - 1);
            
            sum += ++aggSum[i];
            for (int j = i + 1; j <= endIndex; j++)
            {
                aggSum[j] += aggSum[i];
            }
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

        return numOfOwningWinning;
    }
}