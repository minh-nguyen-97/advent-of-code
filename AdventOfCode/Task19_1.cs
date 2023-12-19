namespace AdventOfCode;

public class Task19_1
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var result = ProcessLines(lines.ToArray());
        
        Console.WriteLine(result);
    }

    class Rating
    {
        public int X;
        public int M;
        public int A;
        public int S;

        public Rating(int x, int m, int a, int s)
        {
            X = x;
            M = m;
            A = a;
            S = s;
        }

        public int TotalRating()
        {
            return X + M + A + S;
        }

        public int GetRatingProps(string prop)
        {
            if (prop == "x") return X;
            if (prop == "m") return M;
            if (prop == "a") return A;
            return S;
        }
        
    }

    class Workflows
    {
        public string WorkflowsString;

        public Workflows(string workflowsString)
        {
            WorkflowsString = workflowsString;
        }

        public string GetNextDestination(Rating rating)
        {
            var conditions = WorkflowsString.Split(",");
            for (int i = 0; i < conditions.Length - 1; i++)
            {
                var condition = conditions[i];
                var parts = condition.Split(":");
                var ifCondition = parts[0];
                var satisfyResult = parts[1];
                bool satisfy;
                if (ifCondition.Contains('<'))
                {
                    var ifConditionParts = ifCondition.Split("<");
                    var prop = ifConditionParts[0];
                    var limit = Int32.Parse(ifConditionParts[1]);
                    satisfy = rating.GetRatingProps(prop) < limit;
                }
                else
                {
                    var ifConditionParts = ifCondition.Split(">");
                    var prop = ifConditionParts[0];
                    var limit = Int32.Parse(ifConditionParts[1]);
                    satisfy = rating.GetRatingProps(prop) > limit;
                }

                if (satisfy) return satisfyResult;
                if (!satisfy && i == conditions.Length - 2)
                {
                    return conditions[conditions.Length - 1];
                }
            }

            return null;
        }
    }

    static int ProcessRating(Rating rating, Dictionary<string, Workflows> mapLabelToWorkflows)
    {
        var currentLabel = "in";
        while (currentLabel != "A" && currentLabel != "R")
        {
            var workflow = mapLabelToWorkflows[currentLabel];
            var nextDestination = workflow.GetNextDestination(rating);
            currentLabel = nextDestination;
        }

        if (currentLabel == "A")
        {
            return rating.TotalRating();
        }

        return 0;
    }

    static int ProcessLines(string[] lines)
    {
        var emptyLine = Array.FindIndex(lines, string.IsNullOrEmpty);
        var mapLabelToWorkflows = new Dictionary<string, Workflows>();

        mapLabelToWorkflows["A"] = new Workflows(null);
        mapLabelToWorkflows["R"] = new Workflows(null);
        
        for (int i = 0; i < emptyLine; i++)
        {
            var parts = lines[i].Split("{");
            var label = parts[0];
            var workflows = parts[1].Remove(parts[1].Length - 1);

            if (!mapLabelToWorkflows.ContainsKey(label)) mapLabelToWorkflows[label] = new Workflows(workflows);
        }

        var sum = 0;
        for (int i = emptyLine + 1; i < lines.Length; i++)
        {
            var ratingString = lines[i].Remove(lines[i].Length - 1).Remove(0, 1);
            var ratingParts = ratingString.Split(",").Select(x => Int32.Parse(x.Split("=")[1])).ToArray();
            var rating = new Rating(ratingParts[0], ratingParts[1], ratingParts[2], ratingParts[3]);
            sum += ProcessRating(rating, mapLabelToWorkflows);
        }

        return sum;
    }
}