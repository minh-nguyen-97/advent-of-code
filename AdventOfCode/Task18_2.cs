namespace AdventOfCode;

public class Task18_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }

    static (long, long) GetNextCoord((long, long) currentCoord, string direction, int steps)
    {
        if (direction == "R") return (currentCoord.Item1 + steps, currentCoord.Item2);
        if (direction == "L") return (currentCoord.Item1 - steps, currentCoord.Item2);
        if (direction == "U") return (currentCoord.Item1, currentCoord.Item2 + steps);
        return (currentCoord.Item1, currentCoord.Item2 - steps);
    }

    static string GetDirection(char c)
    {
        var cDigit = c - '0';
        if (cDigit == 0) return "R";
        if (cDigit == 1) return "D";
        if (cDigit == 2) return "L";
        return "U";
    }

    static long ProcessLines(string[] lines)
    {
        long stepsLeft = 0;
        long stepsDown = 0;
        long numOfEdgePoints = 0;
        foreach (var line in lines)
        {
            var colorPart = line.Split(" ")[2];
            var color = colorPart.Remove(colorPart.Length - 1, 1).Remove(0, 2);
            var direction = GetDirection(color[color.Length - 1]);
            var hex = color.Substring(0, color.Length - 1);
            var steps = Convert.ToInt32(hex, 16);

            if (direction == "L") stepsLeft += steps;
            if (direction == "D") stepsDown += steps;
            numOfEdgePoints += steps;
        }

        long currentArea = 0;
        var currentCoord = (stepsLeft + 5, stepsDown + 5);
        foreach (var line in lines)
        {
            var colorPart = line.Split(" ")[2];
            var color = colorPart.Remove(colorPart.Length - 1, 1).Remove(0, 2);
            var direction = GetDirection(color[color.Length - 1]);
            var hex = color.Substring(0, color.Length - 1);
            var steps = Convert.ToInt32(hex, 16);

            var nextCoord = GetNextCoord(currentCoord, direction, steps);
            if (direction == "R")
            {
                var tempArea = Math.Abs(currentCoord.Item1 - nextCoord.Item1) * currentCoord.Item2;
                currentArea += tempArea;
            }
            
            if (direction == "L")
            {
                var tempArea = Math.Abs(currentCoord.Item1 - nextCoord.Item1) * currentCoord.Item2;
                currentArea -= tempArea;
            }

            currentCoord = nextCoord;
        }

        var numOfPointsInside = (currentArea + 1) - (numOfEdgePoints / 2);
        var result = numOfPointsInside + numOfEdgePoints;

        return result;
    }
}