using System.Text;

namespace AdventOfCode;

public class Task10_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }

    class Cell
    {
        public (int, int) Coordinate;
        public Char PipeType;
        public HashSet<(int, int)> StepsFromStart;

        public Cell((int, int) coordinate, Char pipeType, HashSet<(int, int)> stepsFromStart)
        {
            Coordinate = coordinate;
            PipeType = pipeType;
            StepsFromStart = stepsFromStart;
        }
    }

    static int ProcessLines(string[] lines)
    {
        var width = lines[0].Length;
        var height = lines.Length;

        (int, int) startPos = (0, 0);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (lines[i][j] == 'S')
                {
                    startPos = (i, j);
                }
            }
        }

        var result = 0;
        // var pipeType = 'F';
        foreach (var pipeType in reachableList.Keys)
        {
            var numberOfStepsInLoop = HandleStartingType(lines, startPos, pipeType);
            result = Math.Max(result, numberOfStepsInLoop);
        }
        
        return result / 2;
    }

    static int HandleStartingType(string[] lines, (int, int) startPos, Char startPipeType)
    {
        var temp = new StringBuilder(lines[startPos.Item1]);
        temp[startPos.Item2] = startPipeType;
        lines[startPos.Item1] = temp.ToString();
        
        return BFS(lines, startPos, startPipeType);
    }

    static bool IsWithinTheMaze((int, int) coordinate, string[] lines)
    {
        var width = lines[0].Length;
        var height = lines.Length;
        return 0 <= coordinate.Item1 && coordinate.Item1 < height && 0 <= coordinate.Item2 && coordinate.Item2 < width;
    }

    private static Dictionary<(int, int), HashSet<Char>> coordinateToCompatibleMap = new()
    {
        [(-1, 0)] = new() { '|', '7', 'F' },  
        [(1, 0)] = new () { '|', 'L', 'J' },
        [(0, -1)] = new () { '-', 'L', 'F' },  
        [(0, 1)] = new () { '-', 'J', '7' }
    };

    private static Dictionary<Char, List<(int, int)>> reachableList = new()
    {
        ['|'] = new() { (-1, 0), (1, 0) },
        ['-'] = new() { (0, -1), (0, 1) },
        ['L'] = new() { (-1, 0), (0, 1) },
        ['J'] = new() { (-1, 0), (0, -1) },
        ['7'] = new() { (1, 0), (0, -1) },
        ['F'] = new() { (1, 0), (0, 1) },
    };

    static List<(int, int)> GetReachableCellsCoordinates(string[] lines, Cell cell)
    {
        var results = new List<(int, int)>();
        var reachable = reachableList[cell.PipeType];
        foreach (var tuple in reachable)
        {
            var reachableCell = (cell.Coordinate.Item1 + tuple.Item1, cell.Coordinate.Item2 + tuple.Item2);
            if (IsWithinTheMaze(reachableCell, lines) 
                && coordinateToCompatibleMap[tuple].Contains(lines[reachableCell.Item1][reachableCell.Item2]))
            {
                results.Add(reachableCell);
            }
        }

        return results;
    }
    
    static int BFS(string[] lines, (int, int) startPos, Char startPipeType)
    {
        var visitedCells = new Dictionary<(int, int), Cell>();
        var queueingCells = new Queue<Cell>();
        var startCell = new Cell(startPos, startPipeType, new HashSet<(int, int)>(){ startPos });
        queueingCells.Enqueue(startCell);

        var numberOfStepsInLoop = 0;

        while (queueingCells.Count > 0)
        {
            var currentCell = queueingCells.Dequeue();
            if (!visitedCells.ContainsKey(currentCell.Coordinate))
            {
                visitedCells[currentCell.Coordinate] = currentCell;
                var reachableCellsCoordinates = GetReachableCellsCoordinates(lines, currentCell);
                foreach (var nextCellCoor in reachableCellsCoordinates)
                {
                    if (!visitedCells.ContainsKey(nextCellCoor))
                    {
                        var pipeType = lines[nextCellCoor.Item1][nextCellCoor.Item2];
                        var nextStepsFromStart = new HashSet<(int, int)>(currentCell.StepsFromStart);
                        nextStepsFromStart.Add(nextCellCoor);
                        queueingCells.Enqueue(new Cell(
                            nextCellCoor, 
                            pipeType, 
                            nextStepsFromStart));
                    }

                    if (visitedCells.ContainsKey(nextCellCoor) 
                        && currentCell.StepsFromStart.Count > 1 
                        && !currentCell.StepsFromStart.Contains(nextCellCoor))
                    {
                        var nextCell = visitedCells[nextCellCoor];
                        numberOfStepsInLoop = currentCell.StepsFromStart.Count + nextCell.StepsFromStart.Count - 1;
                        break;
                    }
                }
            }

            if (numberOfStepsInLoop > 0) break;
        }

        return numberOfStepsInLoop;
    }
}