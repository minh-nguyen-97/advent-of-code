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

        var result = BFS(lines, startPoint, endPoint);

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

    static int BFS(string[] lines, (int, int) startPoint, (int, int) endPoint)
    {
        var height = lines.Length;
        var width = lines[0].Length;
        var queue = new Queue<(int, int)>();
        var F = new int[height, width];
        var visited = new SortedSet<(int, int)>();
        var trace = new (int, int)[height, width];

        queue.Enqueue(startPoint);
        visited.Add(startPoint);
        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();
            var (currentI, currentJ) = currentPoint;

            var compatibleDirections = FindCompatibleDirection(lines[currentI][currentJ]);
            for (int d = 0; d < compatibleDirections.Length; d++)
            {
                var dIndex = compatibleDirections[d];
                var (nextI, nextJ) = (currentI + directions[dIndex].Item1, currentJ + directions[dIndex].Item2);
                if (isWithinBound(nextI, nextJ, height, width))
                {
                    if (lines[nextI][nextJ] != '#')
                    {
                        var nextPoint = (nextI, nextJ);
                        if (!visited.Contains(nextPoint))
                        {
                            visited.Add(nextPoint);
                            trace[nextI, nextJ] = currentPoint;
                            F[nextI, nextJ] = F[currentI, currentJ] + 1;
                            queue.Enqueue(nextPoint);
                        }
                        else
                        {
                            if (F[currentI, currentJ] + 1 > F[nextI, nextJ] 
                                && (trace[currentI, currentJ].Item1 != nextI 
                                || trace[currentI, currentJ].Item2 != nextJ))
                            {
                                F[nextI, nextJ] = F[currentI, currentJ] + 1;
                                trace[nextI, nextJ] = (currentI, currentJ);
                                queue.Enqueue(nextPoint);
                            }
                        }
                    }
                }
            }
        }
        
        // PrintF(height, width, F);

        var tempPoint = endPoint;
        var steps = 0;

        while (tempPoint.Item1 != startPoint.Item1 || tempPoint.Item2 != startPoint.Item2)
        {
            steps++;
            tempPoint = trace[tempPoint.Item1, tempPoint.Item2];
        }

        return steps;
    }
    
    static bool isWithinBound(int i, int j, int height, int width)
    {
        return 0 <= i && i < height && 0 <= j && j < width;
    }

    static void PrintF(int height, int width, int[,] F)
    {
        Console.WriteLine("F[,] = ");
        Console.Write($"{"", -4}");
        for (int j = 0; j < width; j++)
        {
            Console.Write($"{j, -4}");
        }
        Console.WriteLine();
        for (int i = 0; i < height; i++)
        {
            Console.Write($"{i,-4}");
            for (int j = 0; j < width; j++)
            {
                Console.Write($"{F[i,j], -4}");
            }
            Console.WriteLine();
        }
    }
    
        
    // Console.WriteLine("trace[,] = ");
    // for (int i = 0; i < height; i++)
    // {
    //     for (int j = 0; j < width; j++)
    //     {
    //         var tempTrace = $"({trace[i, j].Item1}, {trace[i, j].Item2})";
    //         Console.Write($"{tempTrace, -10}");
    //     }
    //     Console.WriteLine();
    // }
}