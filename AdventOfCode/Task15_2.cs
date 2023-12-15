using System.Text;

namespace AdventOfCode;

public class Task15_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var line = lines.ToList()[0];
        var result = ProcessLine(line);
        
        Console.WriteLine(result);
    }
    
    static int ProcessLine(string line)
    {
        var sequences = line.Split(",");
        var boxes = new List<string>[256];
        for (int i = 0; i < 256; i++)
        {
            boxes[i] = new List<string>();
        }
        
        var labelInBox = new HashSet<string>();
        var labelToFocalLength = new Dictionary<string, int>();
        foreach (var sequence in sequences)
        {
            if (sequence.IndexOf('-') != -1)
            {
                var minusPos = sequence.IndexOf('-');
                var label = sequence.Substring(0, minusPos);
                var relevantBoxNumber = GetHash(label);
                if (labelInBox.Contains(label))
                {
                    boxes[relevantBoxNumber].Remove(label);
                    labelInBox.Remove(label);
                }
            }
            else
            {
                var equalPos = sequence.IndexOf('=');
                var label = sequence.Substring(0, equalPos);
                var relevantBoxNumber = GetHash(label);
                var focalLengthString = sequence.Substring(equalPos + 1, sequence.Length - 1 - equalPos);
                var focalLength = Int32.Parse(focalLengthString);
                if (!labelInBox.Contains(label))
                {
                    boxes[relevantBoxNumber].Add(label);
                    labelInBox.Add(label);
                }

                labelToFocalLength[label] = focalLength;
            }
        }

        var sum = 0;
        for (int i = 0; i < 256; i++)
        {
            if (boxes[i].Count > 0)
            {
                for (int slot = 0; slot < boxes[i].Count; slot++)
                {
                    var label = boxes[i][slot];
                    var focusPower = (i + 1) * (slot + 1) * labelToFocalLength[label];
                    sum += focusPower;
                }
            }
        }

        return sum;
    }
    
    static int GetHash(string s)
    {
        var asciiValues =  Encoding.ASCII.GetBytes(s);
        var currentValue = 0;
        for (int i = 0; i < s.Length; i++)
        {
            currentValue = ((currentValue + asciiValues[i]) * 17) % 256;
        }

        return currentValue;
    }
}