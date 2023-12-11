namespace AdventOfCode;

public class Task11_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }

    static int ProcessLines(string[] lines)
    {
        var validRows = new SortedDictionary<int, int>();
        var validCols = new SortedDictionary<int, int>();

        var width = lines[0].Length;
        var height = lines.Length;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (lines[i][j] == '#')
                {
                    if (!validRows.ContainsKey(i)) validRows[i] = 0;
                    if (!validCols.ContainsKey(j)) validCols[j] = 0;
                    validRows[i]++;
                    validCols[j]++;
                }
            }
        }

        var resultRow = DP(validRows);
        var resultCol = DP(validCols);

        return resultRow + resultCol;
        // return resultRow;
    }

    static int DP(SortedDictionary<int, int> map)
    {
        var a = map.Keys.ToArray();
        
        var D = new int[a.Length];
        for (int i = 1; i < a.Length; i++)
        {
            var distanceBetweenTwo = (a[i] - a[i - 1] - 1) * 2 + 1;
            D[i] = distanceBetweenTwo;
        }

        var sum = 0;
        for (int i = 1; i < a.Length; i++)
        {
            var distance = D[i];
            for (int j = i-1; j >= 0; j--)
            {
                sum += map[a[i]] * map[a[j]] * distance;
                distance += D[j];
            }
        }

        return sum;
    }
}