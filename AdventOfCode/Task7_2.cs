using System.Text;

namespace AdventOfCode;

public class Task7_2
{
    public static void ProcessFile()
    {
        var lines = File.ReadLines("../../../input.txt");

        var sum = ProcessLines(lines.ToArray());
        Console.WriteLine(sum);
    }

    enum HandType
    {
        HighCard,
        OnePair,
        TwoPairs,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }

    static string ConvertHandToComparableHand(string hand)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i] == 'J') sb.Append('1');
            if (char.IsDigit(hand[i])) sb.Append(hand[i]);
            if (hand[i] == 'T') sb.Append('A');
            if (hand[i] == 'Q') sb.Append('B');
            if (hand[i] == 'K') sb.Append('C');
            if (hand[i] == 'A') sb.Append('D');
        }

        return sb.ToString();
    }

    static HandType GetHandType(string cardHand)
    {
        var counts = new Dictionary<char, int>();
        foreach (var c in cardHand)
        {
            counts.TryAdd(c, 0);
            counts[c] += 1;
        }

        if (counts.Count == 1) return HandType.FiveOfAKind;

        if (counts.ContainsKey('J'))
        {
            var numOfJokers = counts['J'];
            var maxDiffChar = cardHand[0];
            var maxCount = 0;
            foreach (var c in counts.Keys)
            {
                if (c != 'J' && counts[c] > maxCount)
                {
                    maxCount = counts[c];
                    maxDiffChar = c;
                } 
            }

            counts[maxDiffChar] += numOfJokers;
            counts.Remove('J');
        }

        if (counts.Count == 1) return HandType.FiveOfAKind;
        if (counts.Count == 2)
        {
            if (counts.ContainsValue(4)) return HandType.FourOfAKind;
            return HandType.FullHouse;
        }

        if (counts.Count == 3)
        {
            if (counts.ContainsValue(3)) return HandType.ThreeOfAKind;
            return HandType.TwoPairs;
        }

        if (counts.Count == 4) return HandType.OnePair;
        return HandType.HighCard;
    }

    class CardHand
    {
        public string Hand;
        public HandType HandType;
        public int Bid;

        public CardHand(string cardHand, int bid)
        {
            HandType = GetHandType(cardHand);
            Hand = ConvertHandToComparableHand(cardHand);
            Bid = bid;
        }
    }

    static long ProcessLines(string[] lines)
    {
        var handList = new List<CardHand>();
        foreach (var line in lines)
        {
            var parts = line.Split(" ");
            var hand = parts[0];
            var bid = Int32.Parse(parts[1]);
            var cardHand = new CardHand(hand, bid);

            handList.Add(cardHand);
        }
        
        handList.Sort((x, y) =>
        {
            if (x.HandType.CompareTo(y.HandType) == 0)
            {
                return x.Hand.CompareTo(y.Hand);
            }

            return x.HandType.CompareTo(y.HandType);
        });

        long sum = 0;
        for (int i = 0; i < handList.Count; i++)
        {
            sum += handList[i].Bid * (i + 1);
        }
        
        return sum;
    }
    
}