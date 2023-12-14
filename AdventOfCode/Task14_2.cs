using System.Text;

namespace AdventOfCode;

public class Task14_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var grid = lines.ToList();
        grid.Add(CreateBlockLine(grid[0].Length));
        var result = ProcessGrid(grid.ToArray());
        
        Console.WriteLine(result);
    }

    static int ProcessGrid(string[] grid)
    {
        var width = grid[0].Length;
        var height = grid.Length;
        var realHeight = height - 1;
        var sum = 0;
        for (int j = 0; j < width; j++)
        {
            var tempRocks = 0;
            var currentBlock = -1;
            for (int i = 0; i < height; i++)
            {
                if (grid[i][j] == 'O') tempRocks++;
                if (grid[i][j] == '#')
                {
                    var currentStart = currentBlock + 1;
                    for (int k = 0; k < tempRocks ; k++)
                    {
                        var tiltedRockIdx = currentStart + k;
                        var load = realHeight - tiltedRockIdx;
                        sum += load;
                    }

                    currentBlock = i;
                    tempRocks = 0;
                }
            }
        }

        return sum;
    }

    static string CreateBlockLine(int length)
    {
        var s = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            s.Append("#");
        }

        return s.ToString();
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