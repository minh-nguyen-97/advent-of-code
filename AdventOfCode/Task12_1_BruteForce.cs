namespace AdventOfCode;

public class Task12_1_BruteForce
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var sum = 0;
        foreach (var line in lines)
        {
            sum += ProcessLine(line);
        }
        
        Console.WriteLine(sum);
    }

    static int ProcessLine(string line)
    {
        var parts = line.Split(" ");
        var chain = "." + parts[0];
        var b = parts[1].Split(",").Select(x => Int32.Parse(x)).ToArray();
        
        var a = new int[chain.Length];
        var questions = new List<int>();
        for (int i = 0; i < chain.Length; i++)
        {
            if (chain[i] == '.') a[i] = 0;
            if (chain[i] == '#') a[i] = 1;
            if (chain[i] == '?') questions.Add(i);
        }


        var questionsPos = questions.ToArray();
        var status = new int[questionsPos.Length];
        var result = DFS(0, status, questionsPos.Length, questionsPos, a, b);
        return result;
    }

    static bool Check(int[] a, int[] b)
    {
        var continuous = new List<int>();
        var temp = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] == 1)
            {
                temp++;
                if (i == a.Length - 1)
                {
                    continuous.Add(temp);
                }
            }
            if (a[i] == 0 && temp > 0)
            {
                continuous.Add(temp);
                temp = 0;
            }
        }

        var continuousArr = continuous.ToArray();
        if (continuousArr.Length != b.Length) return false;
        for (int i = 0; i < b.Length; i++)
        {
            if (continuousArr[i] != b[i]) return false;
        }

        return true;
    }

    static bool SatisfyCount(int[] status, int[] questionsPos, int[] a, int[] b)
    {
        for (int i = 0; i < questionsPos.Length; i++)
        {
            a[questionsPos[i]] = status[i];
        }

        return Check(a, b);
    }

    static int DFS(int currentStatusIndex, int[] status, int statusLength, int[] questionsPos, int[] a, int[] b)
    {
        if (currentStatusIndex == statusLength)
        {
            if (SatisfyCount(status, questionsPos, a, b)) return 1;
            return 0;
        }

        var sum = 0;
        for (int tempState = 0; tempState < 2; tempState++)
        {
            status[currentStatusIndex] = tempState;
            sum += DFS(currentStatusIndex + 1, status, statusLength, questionsPos, a, b);
        }

        return sum;
    }
}