namespace AdventOfCode;

public class Task23_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }
    
    static int ProcessLines(string[] lines)
    {
        var height = lines.Length;
        var width = lines[0].Length;

        (int, int) startPoint = (0, 0), endPoint = (0, 0);

        for (int j = 0; j < width; j++)
        {
            if (lines[0][j] == '.') startPoint = (0, j);
            if (lines[height - 1][j] == '.') endPoint = (height - 1, j);
        }

        var visited = new SortedSet<(int, int)>();
        visited.Add(startPoint);

        var result = DFS(startPoint, 0, visited, lines, endPoint);

        return result;
    }
    
    // direction 0 = From North to South
    // direction 1 = From West to East
    // direction 2 = From East to West
    // direction 3 = From South to North
    private static readonly (int, int)[] directions = { (1, 0), (0, 1), (0, -1), (-1, 0) };

    static int[] FindCompatibleDirection(char point)
    {
        if (point == 'v') return new[] { 0 };
        if (point == '>') return new[] { 1 };
        if (point == '<') return new[] { 2 };
        if (point == '^') return new[] { 3 };
        return new[] { 0, 1, 2, 3 };
    }

    static int DFS((int, int) currentPoint, int currentSteps, SortedSet<(int, int)> visited, string[] lines, (int, int) endPoint)
    {
        if (currentPoint.Item1 == endPoint.Item1 && currentPoint.Item2 == endPoint.Item2)
        {
            return currentSteps;
        }

        var longestPath = 0;
        var compatibleDirections = FindCompatibleDirection(lines[currentPoint.Item1][currentPoint.Item2]);
        for (int dIndex = 0; dIndex < compatibleDirections.Length; dIndex++)
        {
            var nextD = compatibleDirections[dIndex];
            var nextPoint = (currentPoint.Item1 + directions[nextD].Item1, currentPoint.Item2 + directions[nextD].Item2);
            if (isWithinBound(nextPoint.Item1, nextPoint.Item2, lines.Length, lines[0].Length)
                && !visited.Contains(nextPoint) && lines[nextPoint.Item1][nextPoint.Item2] != '#')
            {
                visited.Add(nextPoint);
                var longestPathNextPoint = DFS(nextPoint, currentSteps + 1, visited, lines, endPoint);
                longestPath = Math.Max(longestPath, longestPathNextPoint);
                visited.Remove(nextPoint);
            }
        }

        return longestPath;
    }
    
    static bool isWithinBound(int i, int j, int height, int width)
    {
        return 0 <= i && i < height && 0 <= j && j < width;
    }
}