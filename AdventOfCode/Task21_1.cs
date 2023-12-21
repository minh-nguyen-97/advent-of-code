namespace AdventOfCode;

public class Task21_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessGrid(lines.ToArray());
        
        Console.WriteLine(result);
    }

    static int ProcessGrid(string[] grid)
    {
        var height = grid.Length;
        var width = grid[0].Length;
        var queue = new Queue<(int, int, int)>();
        var visited = new HashSet<(int, int, int)>();
        
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[i][j] == 'S')
                {
                    visited.Add((i, j, 0));
                    queue.Enqueue((i, j, 0));
                }
            }
        }

        var result = BFS(grid, queue, visited);
        
        return result;
    }
    
    static readonly int MAX_NUM_STEPS = 64;
    private static readonly int MAX_NUM_DIRECTIONS = 4;
    private static readonly (int, int)[] directions = { (1, 0), (0, 1), (0, -1), (-1, 0) };
    
    static bool isWithinBound(int i, int j, int height, int width)
    {
        return 0 <= i && i < height && 0 <= j && j < width;
    }

    static int BFS(string[] grid, Queue<(int, int, int)> queue, HashSet<(int, int, int)> visited)
    {
        var height = grid.Length;
        var width = grid[0].Length;
        var result = 0;
        while (queue.Count > 0)
        {
            var (currentI, currentJ, currentSteps) = queue.Dequeue();
            if (currentSteps == MAX_NUM_STEPS)
            {
                result++;
                continue;
            }

            for (int d = 0; d < MAX_NUM_DIRECTIONS; d++)
            {
                var (nextI, nextJ) = (currentI + directions[d].Item1, currentJ + directions[d].Item2);
                if (isWithinBound(nextI, nextJ, height, width))
                {
                    var nextSteps = currentSteps + 1;
                    var nextNode = (nextI, nextJ, nextSteps);
                    if (!visited.Contains(nextNode) && grid[nextI][nextJ] != '#')
                    {
                        visited.Add(nextNode);
                        queue.Enqueue(nextNode);
                    }
                }
            }
        }

        return result;
    }
}