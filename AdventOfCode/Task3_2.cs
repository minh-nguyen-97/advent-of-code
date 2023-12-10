namespace AdventOfCode;

public class Task3_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../example.txt");

        var sum = 0;
        var prevLine = "";
        Dictionary<int, NumberInLine> prevLinePosToNumberMap = new Dictionary<int, NumberInLine>();
        foreach (var line in lines)
        {
            var (sumOfPrevLine, currentPosToNumberMap)= SumOfValidNumberInPreviousLine(line, prevLine, prevLinePosToNumberMap);
            sum += sumOfPrevLine;

            prevLine = line;
            prevLinePosToNumberMap = currentPosToNumberMap;
        }

        var sumOfLastLine = CalculateValidNumbers(prevLinePosToNumberMap);
        sum += sumOfLastLine;

        Console.WriteLine(sum);
    }

    class NumberInLine
    {
        public int StartPos;
        public int EndPos;
        public int Value;
        public bool IsValid;
        public bool IsAddedToTotalSum;

        public NumberInLine(int startPos, int endPos, int value)
        {
            StartPos = startPos;
            EndPos = endPos;
            Value = value;
            IsValid = false;
            IsAddedToTotalSum = false;
        }

        public void SetIsValid()
        {
            if (!IsValid) IsValid = true;
        }
    }

    static bool IsSymbol(char c)
    {
        return !Char.IsDigit(c) && c != '.';
    }

    static bool IsValidAdjacent(char c1, char c2)
    {
        return (Char.IsDigit(c1) && IsSymbol(c2)) || (Char.IsDigit(c2) && IsSymbol(c1));
    }
    
    static (int, Dictionary<int, NumberInLine>) SumOfValidNumberInPreviousLine(string line, string prevLine, Dictionary<int, NumberInLine> prevLinePosToNumberMap)
    {
        var numbersInLine = GetAllNumbersInLine(line);
        var posToNumberMap = GetPosToNumberMap(numbersInLine);

        for (int i = 0; i < line.Length; i++)
        {
            // Check current line symbol with current line digit
            if (IsSymbol(line[i]))
            {
                if (i > 0 && IsValidAdjacent(line[i], line[i - 1]))
                {
                    posToNumberMap[i - 1].SetIsValid();
                }

                if (i < line.Length - 1 && IsValidAdjacent(line[i], line[i + 1]))
                {
                    posToNumberMap[i + 1].SetIsValid();
                }
            }

            // Check current line symbol with previous line digit
            if (IsSymbol(line[i]))
            {
                for (int step = -1; step <= 1; step++)
                {
                    var j = i + step;
                    if (0 <= j && j < prevLine.Length)
                    {
                        if (IsValidAdjacent(line[i], prevLine[j]))
                        {
                            prevLinePosToNumberMap[j].SetIsValid();
                        }
                    }
                }
            }
            
            // Check current line digit with previous line symbol
            if (Char.IsDigit(line[i]))
            {
                for (int step = -1; step <= 1; step++)
                {
                    var j = i + step;
                    if (0 <= j && j < prevLine.Length)
                    {
                        if (IsValidAdjacent(line[i], prevLine[j]))
                        {
                            posToNumberMap[i].SetIsValid();
                        }
                    }
                }
            }
        }

        var result = CalculateValidNumbers(prevLinePosToNumberMap);

        return (result, posToNumberMap);
    }

    static int CalculateValidNumbers(Dictionary<int, NumberInLine> prevLinePosToNumberMap)
    {
        var result = 0;
        foreach (var numberInPrevLine in prevLinePosToNumberMap.Values)
        {
            if (numberInPrevLine.IsValid && !numberInPrevLine.IsAddedToTotalSum)
            {
                result += numberInPrevLine.Value;
                numberInPrevLine.IsAddedToTotalSum = true;
            }
        }

        return result;
    }

    static Dictionary<int, NumberInLine> GetPosToNumberMap(List<NumberInLine> numbersInLine)
    {
        var posToNumberMap = new Dictionary<int, NumberInLine>();
        foreach (var numberInLine in numbersInLine)
        {
            for (int i = numberInLine.StartPos; i <= numberInLine.EndPos; i++)
            {
                posToNumberMap[i] = numberInLine;
            }
        }

        return posToNumberMap;
    }

    static List<NumberInLine> GetAllNumbersInLine(string line)
    {
        var numbers = new List<NumberInLine>();
        int startPos = -1;
        
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) && startPos < 0)
            {
                startPos = i;
            }

            if (startPos >= 0)
            {
                if (!Char.IsDigit(line[i]) || i == line.Length - 1)
                {
                    var endPos = !Char.IsDigit(line[i]) ? i - 1 : line.Length - 1;
                    var valueString = line.Substring(startPos, endPos - startPos + 1);
                    var value = Int32.Parse(valueString);
                    numbers.Add(new NumberInLine(startPos, endPos, value));

                    startPos = -1;
                } 
            }
        }

        return numbers;
    }
}