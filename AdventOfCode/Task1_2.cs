namespace AdventOfCode;

public class Task1_2
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

    static readonly string[] letterDigits = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    static int IsDigitAndLetter(string line, int currentPos)
    {
        if (Char.IsDigit(line[currentPos])) return line[currentPos] - '0';
        
        for (int i = 0; i < letterDigits.Length; i++)
        {
            var realDigit = i + 1;
            var letterLength = letterDigits[i].Length;
            if (currentPos + letterLength - 1 < line.Length)
            {
                if (line.Substring(currentPos, letterLength).Equals(letterDigits[i]))
                {
                    return realDigit;
                } 
            }
        }

        return -1;
    }
    static int ProcessLine(string line)
    {
        var firstDigit = -1;
        for (int i = 0; i < line.Length; i++)
        {
            var digit = IsDigitAndLetter(line, i);
            if (digit > 0)
            {
                firstDigit = digit;
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
            var digit = IsDigitAndLetter(line, i);
            if (digit >= 0)
            {
                lastDigit = digit;
                break;
            }
        }

        if (lastDigit < 0) return 0;

        return firstDigit * 10 + lastDigit;
    }
}