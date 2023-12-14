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

    

    static int ProcessGrid(string[] grid)
    {
        var a = CreateNewGrid(grid);
        RotateNorthSouth(a, true);
        Print(a);
        var result = CalculateLoad(a);
        return result;
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
    
    static void RotateNorthSouth(int[,] a, bool isRotateNorth)
    {
        var width = a.GetLength(1);
        var height = a.GetLength(0);
        for (int j = 0; j < width; j++)
        {
            var numOfTempRocks = 0;
            var currentBlockIndex = isRotateNorth ? -1 : height;

            var iStart = isRotateNorth ? 0 : height - 1;
            var iEnd = isRotateNorth ? height - 1 : 0;
            var iIncrement = isRotateNorth ? 1 : -1;
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
                        a[tiltedRockIdx,j] = 1;
                    }

                    currentBlockIndex = i;
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
            var tempRocks = 0;
            var currentBlock = -1;
            for (int i = 0; i < height; i++)
            {
                if (a[i,j] == 1) tempRocks++;
                if (a[i,j] == 2 || i == height - 1)
                {
                    var currentStart = currentBlock + 1;
                    for (int k = 0; k < tempRocks ; k++)
                    {
                        var tiltedRockIdx = currentStart + k;
                        var load = height - tiltedRockIdx;
                        sum += load;
                    }

                    currentBlock = i;
                    tempRocks = 0;
                }
            }
        }

        return sum;
    }

}

/*
O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....



After 1 cycle:
.....#....
....#...O#
...OO##...
.OO#......
.....OOO#.
.O#...O#.#
....O#....
......OOOO
#...O###..
#..OO#....

After 2 cycles:
.....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#..OO###..
#.OOO#...O

After 3 cycles:
.....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#...O###.O
#.OOO#...O

*/