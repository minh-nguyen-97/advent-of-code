namespace AdventOfCode;

public class Task8_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }

    class Node
    {
        public string Value;
        public string Left;
        public string Right;

        public Node(string value, string left, string right)
        {
            Value = value;
            Left = left;
            Right = right;
        }
    }

    static int ProcessLines(string[] lines)
    {
        var instruction = lines[0];

        var network = new Dictionary<string, Node>();
        for (int i = 2; i < lines.Length; i++)
        {
            var parts = lines[i].Split(" = ");
            var value = parts[0];
            var leftRightParts = parts[1].Replace("(", "").Replace(")", "").Split(", ");
            var left = leftRightParts[0];
            var right = leftRightParts[1];
            var node = new Node(value, left, right);
            network[value] = node;
        }

        return Traverse(instruction, network);
    }

    static int Traverse(string instruction, Dictionary<string, Node> network)
    {
        var currentNode = network["AAA"];
        var steps = 0;
        var instructionIndex = 0;

        while (currentNode.Value != "ZZZ")
        {
            if (instruction[instructionIndex] == 'L') currentNode = network[currentNode.Left];
            if (instruction[instructionIndex] == 'R') currentNode = network[currentNode.Right];
            steps++;
            instructionIndex = (instructionIndex + 1) % instruction.Length;
        }

        return steps;
    }
}