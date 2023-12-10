namespace AdventOfCode;

public class Task3_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var sum = 0;
        var prevLine = "";
        Dictionary<int, NumberInLine> prevLinePosToNumberMap = new Dictionary<int, NumberInLine>();
        Dictionary<int, GearAdjacentNumbers> prevLineGearAdjacentNumbers = new Dictionary<int, GearAdjacentNumbers>();
        foreach (var line in lines)
        {
            var (sumOfPrevLine, currentPosToNumberMap, currentGearAdjacentNumbers)= SumOfValidNumberInPreviousLine(line, prevLine, 
            prevLinePosToNumberMap, prevLineGearAdjacentNumbers);
            sum += sumOfPrevLine;

            prevLine = line;
            prevLinePosToNumberMap = currentPosToNumberMap;
            prevLineGearAdjacentNumbers = currentGearAdjacentNumbers;
        }

        var sumOfLastLine = CalculatePrevLineGearRatio(prevLineGearAdjacentNumbers);
        sum += sumOfLastLine;

        Console.WriteLine(sum);
    }

    class NumberInLine
    {
        public int StartPos;
        public int EndPos;
        public int Value;

        public NumberInLine(int startPos, int endPos, int value)
        {
            StartPos = startPos;
            EndPos = endPos;
            Value = value;
        }
    }

    class GearAdjacentNumbers
    {
        public Dictionary<(int, int, int), NumberInLine> AdjacentNumbers;

        public GearAdjacentNumbers()
        {
            AdjacentNumbers = new Dictionary<(int, int, int), NumberInLine>();
        }
    }

    static bool IsGear(char c)
    {
        return c == '*';
    }

    static bool IsValidAdjacent(char c1, char c2)
    {
        return (Char.IsDigit(c1) && IsGear(c2)) || (Char.IsDigit(c2) && IsGear(c1));
    }
    
    static (
        int, 
        Dictionary<int, NumberInLine>, 
        Dictionary<int, GearAdjacentNumbers>) 
        SumOfValidNumberInPreviousLine(
        string line, string prevLine, 
        Dictionary<int, NumberInLine> prevLinePosToNumberMap, 
        Dictionary<int, GearAdjacentNumbers> prevLineGearAdjacentNumbers)
    {
        var currentLineNumbers = GetAllNumbersInLine(line);
        var currentLinePosToNumberMap = GetPosToNumberMap(currentLineNumbers);
        var currentLineGearAdjacentNumbers = new Dictionary<int, GearAdjacentNumbers>();

        for (int i = 0; i < line.Length; i++)
        {
            // Check current line symbol with current line digit
            if (IsGear(line[i]))
            {
                if (i > 0 && IsValidAdjacent(line[i], line[i - 1]))
                {
                    if (!currentLineGearAdjacentNumbers.ContainsKey(i))
                        currentLineGearAdjacentNumbers[i] = new GearAdjacentNumbers();

                    var number = currentLinePosToNumberMap[i - 1];
                    var key = (0, number.StartPos, number.EndPos);
                    currentLineGearAdjacentNumbers[i].AdjacentNumbers.TryAdd(key, number);
                }

                if (i < line.Length - 1 && IsValidAdjacent(line[i], line[i + 1]))
                {
                    if (!currentLineGearAdjacentNumbers.ContainsKey(i))
                        currentLineGearAdjacentNumbers[i] = new GearAdjacentNumbers();

                    var number = currentLinePosToNumberMap[i + 1];
                    var key = (0, number.StartPos, number.EndPos);
                    currentLineGearAdjacentNumbers[i].AdjacentNumbers.TryAdd(key, number);
                }
            }

            // Check current line symbol with previous line digit
            if (IsGear(line[i]))
            {
                for (int step = -1; step <= 1; step++)
                {
                    var j = i + step;
                    if (0 <= j && j < prevLine.Length)
                    {
                        if (IsValidAdjacent(line[i], prevLine[j]))
                        {
                            if (!currentLineGearAdjacentNumbers.ContainsKey(i))
                                currentLineGearAdjacentNumbers[i] = new GearAdjacentNumbers();

                            var number = prevLinePosToNumberMap[j];
                            var key = (-1, number.StartPos, number.EndPos);
                            currentLineGearAdjacentNumbers[i].AdjacentNumbers.TryAdd(key, number);
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
                            if (!prevLineGearAdjacentNumbers.ContainsKey(j))
                                prevLineGearAdjacentNumbers[j] = new GearAdjacentNumbers();

                            var number = currentLinePosToNumberMap[i];
                            var key = (1, number.StartPos, number.EndPos);
                            prevLineGearAdjacentNumbers[j].AdjacentNumbers.TryAdd(key, number);
                        }
                    }
                }
            }
        }

        var result = CalculatePrevLineGearRatio(prevLineGearAdjacentNumbers);

        return (result, currentLinePosToNumberMap, currentLineGearAdjacentNumbers);
    }

    static int CalculatePrevLineGearRatio(Dictionary<int, GearAdjacentNumbers> prevLineGearAdjacentNumbers)
    {
        var result = 0;
        foreach (var gearAdjacentNumbers in prevLineGearAdjacentNumbers.Values)
        {
            if (gearAdjacentNumbers.AdjacentNumbers.Count == 2)
            {
                var adjacentNumbersArray = gearAdjacentNumbers.AdjacentNumbers.Values.ToArray();
                var ratio = adjacentNumbersArray[0].Value * adjacentNumbersArray[1].Value;
                result += ratio;
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