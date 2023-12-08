namespace AdventOfCode;

public class Task2_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../task2.txt");

        var sum = 0;
        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }

        Console.WriteLine(sum);
    }

    class Status
    {
        public int Red;
        public int Green;
        public int Blue;

        public Status(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public void SetStatus(string color, int numOfCubes)
        {
            if (color == "red") Red = numOfCubes;
            if (color == "green") Green = numOfCubes;
            if (color == "blue") Blue = numOfCubes;
        }

        public void GetMaxStatus(Status otherStatus)
        {
            Red = Math.Max(Red, otherStatus.Red);
            Green = Math.Max(Green, otherStatus.Green);
            Blue = Math.Max(Blue, otherStatus.Blue);
        }

        public bool IsPossible()
        {
            return Red <= 12 && Green <= 13 && Blue <= 14;
        }
    }

    static int ProcessLine(string line)
    {
        var gameParts = line.Split(":");
        
        var gameIdString = gameParts[0].Split(" ")[1];
        var gameId = Int32.Parse(gameIdString);

        var gameStatus = new Status(0, 0, 0);
        var gameGrabs = gameParts[1].Split(";");
        foreach (var gameGrab in gameGrabs)
        {
            var grabStatus = new Status(0, 0, 0);
            var grabParts = gameGrab.Split(",");
            foreach (var grabPart in grabParts)
            {
                var cubeParts = grabPart.Split(" ");
                var numOfCubes = Int32.Parse(cubeParts[1]);
                var cubeColor = cubeParts[2];
                grabStatus.SetStatus(cubeColor, numOfCubes);
            }
            gameStatus.GetMaxStatus(grabStatus);
        }

        return gameStatus.IsPossible() ? gameId : 0;
    }
}