namespace AdventOfCode;

public class Task6_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt").ToArray();

        var times = ParseArray(lines[0]);
        var distances = ParseArray(lines[1]);

        var result = ProcessTimeDistance(times[0], distances[0]);
        for (int i = 1; i < times.Length; i++)
        {
            var numOfWays = ProcessTimeDistance(times[i], distances[i]);
            result *= numOfWays;
        }
        
        Console.WriteLine(result);
    }

    static int ProcessTimeDistance(int totalTime, int goalDistance)
    {
        var middleTime = totalTime / 2;
        if (middleTime * (totalTime - middleTime) <= goalDistance) return 0;
        
        var (start, end) = (1, middleTime);
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

    static int[] ParseArray(string line)
    {
        var numberPart = line.Split(":")[1].Trim();
        var numbers = numberPart.Split(" ").Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(x => Int32.Parse(x.Trim())).ToArray();
        return numbers;
    }
}