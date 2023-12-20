namespace AdventOfCode;

public class Task17_1_Try_DP
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }

    static bool isWithinBound(int i, int j, int height, int width)
    {
        return 0 <= i && i < height && 0 <= j && j < width;
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
        var F = new int[height, width, 4];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                a[i, j] = lines[i][j] - '0';
                for (int k = 0; k < 4; k++)
                {
                    F[i, j, k] = Int32.MaxValue;
                }
            }
        }

        F[0, 0, 0] = 0;
        F[0, 0, 1] = 0;
        HandleNorthToSouthAndWestToEast(height, width, a, F);
        HandleSouthToNorth(height, width, a, F);
        HandleNorthToSouthAndWestToEast(height, width, a, F);
        HandleEastToWest(height, width, a, F);
        HandleNorthToSouthAndWestToEast(height, width, a, F);

        var result = Math.Min(F[height - 1, width - 1, 0], F[height - 1, width - 1, 1]);
        return result;
    }

    static int OptimizeRoute(int height, int width, int[,] a, int[,,] F)
    {
        HandleSouthToNorth(height, width, a, F);
        HandleNorthToSouthAndWestToEast(height, width, a, F);
        HandleEastToWest(height, width, a, F);
        HandleNorthToSouthAndWestToEast(height, width, a, F);

        var result = Math.Min(F[height - 1, width - 1, 0], F[height - 1, width - 1, 1]);
        return result;
    }
    
    static void HandleEastToWest(int height, int width, int[,] a, int[,,] F)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = width - 1; j >= 0; j--)
            {
                var stepsHeat = a[i, j];
                for (int steps = 1; steps <= 3; steps++)
                {
                    if (isWithinBound(i, j + steps, height, width))
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            if (d != 2 && F[i, j + steps, d] != Int32.MaxValue)
                            {
                                F[i, j, 2] = Math.Min(F[i, j, 2], F[i, j + steps, d] + stepsHeat);
                            }
                        }

                        stepsHeat += a[i, j + steps];
                    }
                }
            }
        }
    }

    static void HandleSouthToNorth(int height, int width, int[,] a, int[,,] F)
    {
        for (int i = height - 1; i >= 0; i--)
        {
            for (int j = 0; j < width; j++)
            {
                var stepsHeat = a[i, j];
                for (int steps = 1; steps <= 3; steps++)
                {
                    if (isWithinBound(i + steps, j, height, width))
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            if (d != 3 && F[i + steps, j, d] != Int32.MaxValue)
                            {
                                F[i, j, 3] = Math.Min(F[i, j, 3], F[i + steps, j, d] + stepsHeat);
                            }
                        }

                        stepsHeat += a[i + steps, j];
                    }
                }
            }
        }
    }

    static void HandleNorthToSouthAndWestToEast(int height, int width, int[,] a, int[,,] F)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // Handle steps from North down to South
                var stepsHeat = a[i, j];
                for (int steps = 1; steps <= 3; steps++)
                {
                    if (isWithinBound(i - steps, j, height, width))
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            if (d != 0 && F[i - steps, j, d] != Int32.MaxValue)
                            {
                                F[i, j, 0] = Math.Min(F[i, j, 0], F[i - steps, j, d] + stepsHeat);
                            }
                        }

                        stepsHeat += a[i - steps, j];
                    }
                }
                
                // Handle steps from West to East
                stepsHeat = a[i, j];
                for (int steps = 1; steps <= 3; steps++)
                {
                    if (isWithinBound(i, j - steps, height, width))
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            if (d != 1 && F[i, j - steps, d] != Int32.MaxValue)
                            {
                                F[i, j, 1] = Math.Min(F[i, j, 1], F[i, j - steps, d] + stepsHeat);
                            }
                        }

                        stepsHeat += a[i, j - steps];
                    }
                }
            }
        }
    }
}