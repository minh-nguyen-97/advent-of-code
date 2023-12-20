namespace AdventOfCode;

public class Task17_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }
    
    // direction 0 = From North to South
    // direction 1 = From West to East
    // direction 2 = From East to West
    // direction 3 = From South to North
    static int ProcessLines(string[] lines)
    {
        var height = lines.Length;
        var width = lines[0].Length;
        var a = new int[height, width];
        var F = new int[height, width, 4, 4];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                a[i, j] = lines[i][j] - '0';
                for (int d = 0; d < 4; d++)
                {
                    for (int steps = 1; steps <= 3; steps++)
                    {
                        F[i, j, d, steps] = Int32.MaxValue;
                    }
                }
            }
        }

        var result = Dijkstra(height, width, a, F);

        return result;
    }

    private static readonly (int, int)[] directions = { (1, 0), (0, 1), (0, -1), (-1, 0) };
    private static readonly int[,] compatibleDirections = { { 0, 1, 2 }, { 0, 1, 3 }, { 0, 2, 3 }, { 1, 2, 3 } };

    static int Dijkstra(int height, int width, int[,] a, int[,,,] F)
    {
        var heap = new PriorityQueue<(int, int, int, int), int>();
        F[0, 0, 0, 0] = 0;
        F[0, 0, 1, 0] = 0;
        heap.Enqueue((0, 0, 0, 0), 0);
        heap.Enqueue((0, 0, 1, 0), 0);

        var trace = new (int, int, int, int)[height, width, 4, 4];

        while (heap.Count > 0)
        {
            var (currentI, currentJ, currentD, currentSteps) = heap.Dequeue();
            if (currentI == height - 1 && currentJ == width - 1)
            {
                // trace
                var (tempI, tempJ, tempD, tempSteps) = (currentI, currentJ, currentD, currentSteps);
                var printTraces = new List<(int, int, int, int)>();
                while (tempI != 0 || tempJ != 0)
                {
                    printTraces.Add((tempI, tempJ, tempD, tempSteps));
                    (tempI, tempJ, tempD, tempSteps) = trace[tempI, tempJ, tempD, tempSteps];
                }
                printTraces.Add((tempI, tempJ, tempD, tempSteps));

                for (int i = printTraces.Count - 1; i>=0; i--)
                {
                    Console.WriteLine($"({printTraces[i].Item1}, {printTraces[i].Item2}, {printTraces[i].Item3}, {printTraces[i].Item4})");
                }
                
                return F[currentI, currentJ, currentD, currentSteps];
            }
            
            for (int dIndex = 0; dIndex < 3; dIndex++)
            {
                var nextD = compatibleDirections[currentD, dIndex];
                var dIncrement = directions[nextD];
                var startingStepsForNextDirection = nextD == currentD ? currentSteps : 0;
                var stepsHeat = 0;
                for (int steps = 1; steps <= 3; steps++)
                {
                    var nextI = currentI + steps * dIncrement.Item1;
                    var nextJ = currentJ + steps * dIncrement.Item2;
                    var nextSteps = startingStepsForNextDirection + steps;
                    if (nextSteps <= 3 && isWithinBound(nextI, nextJ, height, width))
                    {
                        stepsHeat += a[nextI, nextJ];
                        if (F[currentI, currentJ, currentD, currentSteps] + stepsHeat <
                            F[nextI, nextJ, nextD, nextSteps])
                        {
                            F[nextI, nextJ, nextD, nextSteps] =
                                F[currentI, currentJ, currentD, currentSteps] + stepsHeat;
                            heap.Enqueue((nextI, nextJ, nextD, nextSteps), F[nextI, nextJ, nextD, nextSteps]);
                            
                            // trace
                            trace[nextI, nextJ, nextD, nextSteps] = (currentI, currentJ, currentD, currentSteps);
                        }
                    }
                }
            }
        }

        return -1;
    }
    
    static bool isWithinBound(int i, int j, int height, int width)
    {
        return 0 <= i && i < height && 0 <= j && j < width;
    }
}

/*
              111
    0123456789012
  0 2413432311323
  1 3215453535623
  2 3255245654254
  3 3446585845452
  4 4546657867536
  5 1438598798454
  6 4457876987766
  7 3637877979653
  8 4654967986887
  9 4564679986453
 10 1224686865563
 11 2546548887735
 12 4322674655533
*/