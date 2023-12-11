namespace AdventOfCode;

public class Task9_2
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
        var numbers = line.Split(" ")
            .Where(x => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrEmpty(x))
            .Select(x => Int32.Parse(x.Trim()))
            .ToArray();

        return CalculateNextNumber(numbers);
    }

    static bool IsAllNumbersEqual(int[] numbers)
    {
        var value = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] != value) return false;
        }

        return true;
    }
    
    static int CalculateNextNumber(int[] numbers)
    {
        if (IsAllNumbersEqual(numbers)) return numbers[0];

        var nextArr = new int[numbers.Length - 1];
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            nextArr[i] = numbers[i + 1] - numbers[i];
        }

        var nextArrNextNumber = CalculateNextNumber(nextArr);
        return numbers[numbers.Length - 1] + nextArrNextNumber;
    }
}