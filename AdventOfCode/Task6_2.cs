using System.Text;

namespace AdventOfCode;

public class Task6_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt").ToArray();

        var time = ParseNumber(lines[0]);
        var distance = ParseNumber(lines[1]);

        var result = ProcessTimeDistance(time, distance);
        
        Console.WriteLine(result);
    }

    static long ProcessTimeDistance(long totalTime, long goalDistance)
    {
        var middleTime = totalTime / 2;
        if (middleTime * (totalTime - middleTime) <= goalDistance) return 0;
        
        var (start, end) = ((long)1, middleTime);
        while (start <= end)
        {
            var mid = (start + end) / 2;
            if (mid * (totalTime - mid) <= goalDistance)
            {
                start = mid + 1;
            }
            else
            {
                end = mid - 1;
            }
        }

        var breakPoint = end;
        var result = totalTime - 1 - breakPoint * 2;

        return result;
    }

    static long ParseNumber(string line)
    {
        var numberPart = line.Split(":")[1].Trim();
        var numbers = numberPart.Split(" ").Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
        .Select(x => x.Trim());
        var bigNumber = new StringBuilder();
        foreach (var number in numbers)
        {
            bigNumber.Append(number);
        }
        return long.Parse(bigNumber.ToString());
    }
}