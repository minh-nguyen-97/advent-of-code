namespace AdventOfCode;

public class Task18_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }

    static (int, int) GetNextCoord((int, int) currentCoord, string direction, int steps)
    {
        if (direction == "R") return (currentCoord.Item1 + steps, currentCoord.Item2);
        if (direction == "L") return (currentCoord.Item1 - steps, currentCoord.Item2);
        if (direction == "U") return (currentCoord.Item1, currentCoord.Item2 + steps);
        return (currentCoord.Item1, currentCoord.Item2 - steps);
    }

    static int ProcessLines(string[] lines)
    {
        var stepsLeft = 0;
        var stepsDown = 0;
        var numOfEdgePoints = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(" ");
            var direction = parts[0];
            var steps = Int32.Parse(parts[1]);

            if (direction == "L") stepsLeft += steps;
            if (direction == "D") stepsDown += steps;
            numOfEdgePoints += steps;
        }

        var currentArea = 0;
        var currentCoord = (stepsLeft + 5, stepsDown + 5);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var parts = line.Split(" ");
            var direction = parts[0];
            var steps = Int32.Parse(parts[1]);

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