using System.Text;

namespace AdventOfCode;

public class Task14_2
{
    // '.' = 0
    // 'O' = 1
    // '#' = 2
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessGrid(lines.ToArray());
        
        Console.WriteLine(result);
    }

    static int[,] CreateNewGrid(string[] grid)
    {
        var a = new int[grid.Length, grid[0].Length];
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                if (grid[i][j] == '.') a[i, j] = 0;
                if (grid[i][j] == 'O') a[i, j] = 1;
                if (grid[i][j] == '#') a[i, j] = 2;
            }
        }

        return a;
    }

    private static readonly int NUM_OF_CYCLES = 1000000000;

    static int ProcessGrid(string[] grid)
    {
        var a = CreateNewGrid(grid);
        var width = a.GetLength(1);
        var height = a.GetLength(0);

        var (trace, loopStartIndex) = DetectLoop(a);
        var loopLength = trace.Count - loopStartIndex;
        var positionInLoop = (NUM_OF_CYCLES - loopStartIndex) % loopLength;
        var finalState = trace[loopStartIndex + positionInLoop];
        var finalStateGrid = ConvertStringToArray(finalState, width, height);

        var result = CalculateLoad(finalStateGrid);
        return result;
    }

    static (List<string>, int) DetectLoop(int[,] a)
    {
        var visitedState = new Dictionary<string, int>();
        var trace = new List<string>();
        var state = ConvertArrayToString(a);
        var num = 0;
        while (!visitedState.ContainsKey(state))
        {
            visitedState.Add(state, num);
            trace.Add(state);
            num++;
            CycleRotate(a);
            state = ConvertArrayToString(a);
        }

        return (trace, visitedState[state]);
    }

    static string ConvertArrayToString(int[,] a)
    {
        var sb = new StringBuilder();
        var width = a.GetLength(1);
        var height = a.GetLength(0);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                sb.Append(a[i, j].ToString());
            }
        }

        return sb.ToString();
    }
    
    static int[,] ConvertStringToArray(string state, int width, int height)
    {
        var a = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                a[i, j] = state[i * height + j] - '0';
            }
        }

        return a;
    }

    static void CycleRotate(int[,] a)
    {
        RotateNorthSouth(a, true);
        RotateWestEast(a, true);
        RotateNorthSouth(a, false);
        RotateWestEast(a, false);
    }
    
    static void RotateNorthSouth(int[,] a, bool isRotateNorth)
    {
        var width = a.GetLength(1);
        var height = a.GetLength(0);
        for (int j = 0; j < width; j++)
        {
            var numOfTempRocks = 0;
            
            var currentBlockIndex = isRotateNorth ? -1 : height;
            var (iStart, iEnd, iIncrement) = isRotateNorth ? (0, height - 1, 1) : (height - 1, 0, -1);
            for (int i = iStart; i != iEnd + iIncrement; i += iIncrement)
            {
                if (a[i, j] == 1)
                {
                    numOfTempRocks++;
                    a[i, j] = 0;
                }
                if (a[i, j] == 2 || i == iEnd)
                {
                    for (int k = 1; k <= numOfTempRocks ; k++)
                    {
                        var tiltedRockIdx = currentBlockIndex + k * iIncrement;
                        a[tiltedRockIdx, j] = 1;
                    }

                    currentBlockIndex = i;
                    numOfTempRocks = 0;
                }
            }
        }
    }
    
    static void RotateWestEast(int[,] a, bool isRotateWest)
    {
        var width = a.GetLength(1);
        var height = a.GetLength(0);
        for (int i = 0; i < height; i++)
        {
            var numOfTempRocks = 0;

            var currentBlockIndex = isRotateWest ? -1 : width;
            var (jStart, jEnd, jIncrement) = isRotateWest ? (0, width - 1, 1) : (width - 1, 0, -1);
            for (int j = jStart; j != jEnd + jIncrement; j += jIncrement)
            {
                if (a[i, j] == 1)
                {
                    numOfTempRocks++;
                    a[i, j] = 0;
                }
                if (a[i, j] == 2 || j == jEnd)
                {
                    for (int k = 1; k <= numOfTempRocks ; k++)
                    {
                        var tiltedRockIdx = currentBlockIndex + k * jIncrement;
                        a[i, tiltedRockIdx] = 1;
                    }

                    currentBlockIndex = j;
                    numOfTempRocks = 0;
                }
            }
        }
    }

    static int CalculateLoad(int[,] a)
    {
        var width = a.GetLength(1);
        var height = a.GetLength(0);
        var sum = 0;
        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < height; i++)
            {
                if (a[i, j] == 1)
                {
                    sum += height - i;
                }
            }
        }

        return sum;
    }

    // Print Helpers
    static void PrintGrid(int[,] a)
    {
        var width = a.GetLength(1);
        var height = a.GetLength(0);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                char c = '.';
                if (a[i, j] == 0) c = '.';
                if (a[i, j] == 1) c = 'O';
                if (a[i, j] == 2) c = '#';
                Console.Write(c);
            }
            Console.WriteLine();
        }
    }
    
    static void Print(int[,] a)
    {
        var width = a.GetLength(1);
        var height = a.GetLength(0);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write(a[i,j]);
            }
            Console.WriteLine();
        }
    }
}