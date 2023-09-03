using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIDeciding : AIAction
{
    public override AIState Execute(List<string> tableCards, List<string> aiHand, Player player)
    {
        Debug.Log(player.CanPlayerRaise());

        if (player.CanPlayerRaise() && (HasStrongHand(tableCards, aiHand) || HasMediocreHand(tableCards, aiHand)))
        {
            return AIState.Raising;
        }
        else if (player.CanPlayerCall() && (HasMediocreHand(tableCards, aiHand) || HasStrongHand(tableCards, aiHand)))
        {
            return AIState.Calling;
        }
        else if (player.CanPlayerCheck() && (HasMediocreHand(tableCards, aiHand) || HasWeakHand(tableCards, aiHand)) )
        {
            return AIState.Checking;
        }
        else if (HasWeakHand(tableCards, aiHand))
        {
            return AIState.Folding;
        }
        else
        {
            return AIState.Folding;
        }
    }

    //Evaluating AI Hand based on their potentional;
    private bool HasWeakHand(List<string> tableCards, List<string> aiHand)
    {
        bool weakHand = false;

        if (tableCards.Count == 0)
        {
            if(!HasHighCard(aiHand) || !HasSequence(aiHand) || !HasPair(aiHand))
                weakHand = true;
        }
        else if (tableCards.Count > 0)
        {
            if (!HasPairs(tableCards, aiHand))
                weakHand = true;
        }

        if (!weakHand)
            Debug.Log("WeakHand !");

        return weakHand;
    }

    private bool HasStrongHand(List<string> tableCards, List<string> aiHand)
    {
        bool strongHand = false;

        if (tableCards.Count == 0)
        {
            if (HasSequence(aiHand) || HasPair(aiHand))
                strongHand = true;
        }
        else if(tableCards.Count > 0)
        {
            if (HasRoyalStraightFlush(tableCards, aiHand) || HasStraightFlush(tableCards, aiHand) || HasStraightFlush(tableCards, aiHand) || HasFour(tableCards, aiHand) ||
                HasFullHouse(tableCards, aiHand) || HasFlush(tableCards, aiHand) || HasStraight(tableCards, aiHand) || HasThree(tableCards, aiHand) || HasTwoPairs(tableCards, aiHand))
                strongHand = true;
        }

        if (strongHand)
            Debug.Log("StrongHand !");

        return strongHand;
    }

    private bool HasMediocreHand(List<string> tableCards, List<string> aiHand)
    {
        bool mediocreHand = false;

        if (tableCards.Count == 0)
        {
            if (HasHighCard(aiHand) || HasSequence(aiHand))
                mediocreHand = true;
        }
        else if(tableCards.Count > 0)
        {
            if (HasPairs(tableCards, aiHand))
                mediocreHand = true;
        }

        if (mediocreHand)
            Debug.Log("MediocreHand !");

        return mediocreHand;
    }

    //Methods to determine AI Hand combination;
    private bool HasHighCard(List<string> aiHand)
    {
        foreach (string card in aiHand)
        {
            string rank = card.Substring(0, 1);

            if (rank == "J" || rank == "Q" || rank == "K" || rank == "A")
            {
                Debug.Log("HasHighCard");
                return true; // Found a high card.
            }
        }

        return false; // No high cards found.
    }

    private bool HasPair(List<string> aiHand)
    {
        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in aiHand)
        {

            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        if (rankCount.Any(pair => pair.Value == 2))
        {
            Debug.Log("HasPair");
        }

        // Check if any rank has a count of 2 (indicating a pair).
        return rankCount.Any(pair => pair.Value == 2);
    }

    public static bool HasSequence(List<string> aiHand)
    {
        // Create a dictionary to count the occurrences of each rank.
        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in aiHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        // Create an array of possible ranks for a straight.
        string[] possibleRanks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        // Check if there's a sequence of 2 consecutive ranks.
        for (int i = 0; i < possibleRanks.Length - 1; i++)
        {
            if (rankCount.ContainsKey(possibleRanks[i]) && rankCount.ContainsKey(possibleRanks[i + 1]))
            {
                Debug.Log("HasSequence");
                return true;
            }
        }

        return false;
    }

    private bool HasPairs(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        int pairsCount = rankCount.Values.Count(count => count == 2); // Count ranks with a count of 2.

        if (pairsCount >= 1)
        {
            // Check if at least one rank has a pair.
            foreach (string card in aiHand)
            {
                string rank = card.Substring(0, 1);

                if (rankCount.ContainsKey(rank) && rankCount[rank] == 2)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool HasTwoPairs(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        int pairCount = 0;
        bool hasPairInAiHand = false;

        foreach (var count in rankCount.Values)
        {
            if (count == 2)
            {
                pairCount++;
            }
        }

        foreach (var card in aiHand)
        {
            string rank = card.Substring(0, 1);

            if (rankCount.ContainsKey(rank))
            {
                hasPairInAiHand = true;
                break;
            }
        }

        return pairCount >= 2 && hasPairInAiHand;
    }

    private bool HasThree(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        // Create a dictionary to count the occurrences of each rank.
        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        if (rankCount.Values.Any(count => count == 3))
        {
            // Check if at least one card from aiHand is part of the three of a kind group.
            foreach (string card in aiHand)
            {
                string rank = card.Substring(0, 1);

                if (rankCount.ContainsKey(rank) && rankCount[rank] == 3)
                {
                    return true;
                }
            }
        }

        // Check if any rank appears three times (indicating three of a kind).
        return false;
    }

    private bool HasStraight(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        // Create a dictionary to count the occurrences of each rank.
        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        // Create an array of possible ranks for a straight.
        string[] possibleRanks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };

        // Initialize a list to store the cards in the sequence.
        List<string> sequenceCards = new List<string>();

        // Check if there's a sequence of 5 consecutive ranks in the combined hand.
        for (int i = 0; i < possibleRanks.Length - 5; i++)
        {
            int consecutiveCount = 0;
            List<string> currentSequence = new List<string>();

            for (int j = i; j < i + 5; j++)
            {
                if (rankCount.ContainsKey(possibleRanks[j]))
                {
                    consecutiveCount++;
                    currentSequence.Add(possibleRanks[j] + "X"); // "X" just to indicate the rank.
                }
            }

            if (consecutiveCount == 5)
            {
                sequenceCards = currentSequence.Select(rank => rank.Replace("X", "")).ToList();
                return true;
            }
        }

        return false;
    }

    private bool HasFlush(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        // Create a dictionary to count the occurrences of each suit.
        Dictionary<string, int> suitCount = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string suit = card.Substring(1, 1);

            if (!suitCount.ContainsKey(suit))
            {
                suitCount[suit] = 0;
            }

            suitCount[suit]++;
        }

        // Check if any suit appears five times (indicating a flush).
        return suitCount.Values.Any(count => count >= 5);
    }

    private bool HasFullHouse(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        // Create a dictionary to count the occurrences of each rank.
        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        // Check if there is exactly one rank with three occurrences and one rank with two occurrences.
        bool hasThreeOfAKind = rankCount.Values.Any(count => count == 3);
        bool hasOnePair = rankCount.Values.Any(count => count == 2);

        if (hasThreeOfAKind && hasOnePair)
        {
            // Check if at least one card from aiHand is part of the three of a kind or the one pair.
            foreach (string card in aiHand)
            {
                string rank = card.Substring(0, 1);

                if (rankCount.ContainsKey(rank) && (rankCount[rank] == 3 || rankCount[rank] == 2))
                {
                    return true;
                }
            }
        }

        // Check if there is exactly one rank with three occurrences and one rank with two occurrences.
        return false;
    }

    private bool HasFour(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        // Create a dictionary to count the occurrences of each rank.
        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount.ContainsKey(rank))
            {
                rankCount[rank] = 0;
            }

            rankCount[rank]++;
        }

        if (rankCount.Values.Any(count => count == 4))
        {
            // Check if at least one card from aiHand is part of the four of a kind group.
            foreach (string card in aiHand)
            {
                string rank = card.Substring(0, 1);

                if (rankCount.ContainsKey(rank) && rankCount[rank] == 4)
                {
                    return true;
                }
            }
        }

        // Check if any rank appears four times (indicating four of a kind).
        return false;
    }

    private bool HasStraightFlush(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        // Group the cards by suit.
        var groupedBySuit = combinedHand.GroupBy(card => card.Substring(1, 1));

        foreach (var group in groupedBySuit)
        {
            // Create a dictionary to store the presence of ranks in the suit.
            Dictionary<string, bool> rankPresence = new Dictionary<string, bool>();

            foreach (string card in group)
            {
                string rank = card.Substring(0, 1);
                rankPresence[rank] = true;
            }

            // Check for a sequence of 5 consecutive ranks in the suit.
            int consecutiveCount = 0;

            foreach (string rank in rankPresence.Keys.OrderBy(rank => rank))
            {
                if (rankPresence.ContainsKey(rank))
                {
                    consecutiveCount++;
                }
                else
                {
                    consecutiveCount = 0;
                }

                if (consecutiveCount == 5)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool HasRoyalStraightFlush(List<string> tableCards, List<string> aiHand)
    {
        List<string> combinedHand = new List<string>(aiHand);
        combinedHand.AddRange(tableCards);

        // Group the cards by suit.
        var groupedBySuit = combinedHand.GroupBy(card => card.Substring(1, 1));

        foreach (var group in groupedBySuit)
        {
            // Create a dictionary to store the presence of ranks in the suit.
            Dictionary<string, bool> rankPresence = new Dictionary<string, bool>();

            foreach (string card in group)
            {
                string rank = card.Substring(0, 1);
                rankPresence[rank] = true;
            }

            // Check for a sequence of "A", "10", "J", "Q", "K" in the suit.
            List<string> requiredRanks = new List<string> { "A", "10", "J", "Q", "K" };
            int consecutiveCount = 0;

            foreach (string rank in requiredRanks)
            {
                if (rankPresence.ContainsKey(rank))
                {
                    consecutiveCount++;
                }
                else
                {
                    consecutiveCount = 0;
                }

                if (consecutiveCount == 5)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
