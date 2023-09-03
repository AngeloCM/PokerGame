using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Total of Players")]
    [SerializeField] public int NumOfPlayers;
    [SerializeField] public int NumOfAIs;

    [Header("Players")]
    [SerializeField] public GameObject currentPlayer;
    [SerializeField] public List<GameObject> PlayerPanels;
    [SerializeField] public GameObject TablePanel;

    [Header("Total Bets")]
    [SerializeField] public int totalBet;
    [SerializeField] public TMPro.TMP_Text textTotal;
    [SerializeField] public TMPro.TMP_Text Winner;
    [SerializeField] public GameObject ReloadPanel;
    private bool showedWinner = false;

    [Header("Rounds Played")]
    [SerializeField] public bool flop = false;
    [SerializeField] public bool fourth = false;
    [SerializeField] public bool fifth = false;
    [SerializeField] public bool finished = false;
    [SerializeField] public GameObject winner;
    [SerializeField] public GameObject loser;

    [Header("Table Cards")]
    public List<string> table_cards;

    [Header("List Of Cards in the Deck")]
    [SerializeField] List<string> cards;
    Deck pokerDeck;

    Display display;

    // Start is called before the first frame update
    void Start()
    {
        display = GameObject.FindGameObjectWithTag("Players").GetComponent<Display>();

        //Create new Deck of Cards
        pokerDeck = new Deck();
        cards = pokerDeck.PokerDeck;

        //Create List of Card on the table
        table_cards = new List<string>();

        //Setting Num of Players in Display
        display.SetPanels(PlayerPanels, NumOfPlayers, NumOfAIs);

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(totalBet > 0)
            textTotal.text = "Total Bet: " + totalBet.ToString();
    }

    void StartGame()
    {
        Winner.text = "";
        showedWinner = false;

        DealCards();
        SetAIs();
    }

    void SetAIs()
    {
        if(NumOfAIs > 0)
        {
            for (int i = 0; i < PlayerPanels.Count; i++)
            {
                if (PlayerPanels[i].activeSelf && i > 0)
                {
                    PlayerPanels[i].GetComponent<Player>().isAI = true;
                }
            }
        }
    }

    public void CheckWinner()
    {
        if (!showedWinner)
        {
            Winner.text += " " + CheckPlayersHand()[0].GetComponent<Player>().Name;
            ReloadPanel.SetActive(true);

            showedWinner = true;
        }
    }

    void DealCards()
    {
        //Deal cards for players
        foreach (var player in PlayerPanels)
        {
            if (player.activeSelf)
            {
                for (int i = 0; i < 2; i++)
                {
                    player.GetComponent<Player>().addCardToList(cards[i]);
                    cards.Remove(cards[i]);
                }
            }
        }

        //Show Cards for each Player
        display.SetPlayerPanel(PlayerPanels);
    }

    public void DealFlop()
    {
        //Deal cards for the Flop
        for (int i = 0; i < 3; i++)
        {
            table_cards.Add(cards[i]);
            cards.Remove(cards[i]);
        }

        //Show Cards for the Flop
        display.SetCardInFlop(TablePanel, table_cards);

        //Flop on the Table
        flop = true;
    }

    public void DealFourthCard()
    {
        for (int i = 0; i < 1; i++)
        {
            table_cards.Add(cards[i]);
            //Show Fourth Card
            display.SetFourthCard(TablePanel, cards[i]);

            cards.Remove(cards[i]);
        }
        //Fourth on the Table
        fourth = true;
    }

    public void DealFifthCard()
    {
        for (int i = 0; i < 1; i++)
        {
            table_cards.Add(cards[i]);
            //Show Fourth Card
            display.SetFifthCard(TablePanel, cards[i]);

            cards.Remove(cards[i]);
        }

        //Fifth on the Table
        fifth = true;
    }

    public void ResetTurn()
    {
        currentPlayer = null;

        foreach (var player in PlayerPanels)
        {
            if (player.activeSelf)
            {
                Player p = player.GetComponent<Player>();
                p.played = 0;
                p.raised = false;
                p.called = false;
                p.check = false;
                p.Bet = 0;
            }
        }
    }

    public void SetNextPlayerTurn()
    {
        foreach (var player in PlayerPanels)
        {
            if (player.activeSelf && !player.GetComponent<Player>().stopPlaying)
            {
                Player p = player.GetComponent<Player>();

                if(p.played <= 0)
                {
                    p.myTurn = true;
                    currentPlayer = player;
                    player.GetComponent<UIController>().TurnButtons(true);
                    player.gameObject.GetComponent<UIController>().PokerHand.text = "";
                    break;
                }
                else if (p.played > 0 && CheckPlayersBet() && !p.raised)
                {
                    p.myTurn = true;
                    currentPlayer = player;
                    player.GetComponent<UIController>().TurnButtons(true);
                    player.gameObject.GetComponent<UIController>().PokerHand.text = "";
                    break;
                }
            } 
        }
    }

    public bool CheckIfAllPlayed()
    {
        bool everyonePlayed = true;

        foreach (var player in PlayerPanels)
        {
            if (player.activeSelf && !player.GetComponent<Player>().stopPlaying)
            {
                Player p = player.GetComponent<Player>();

                if (p.played <= 0)
                {
                    everyonePlayed = false;
                    break;
                }
            }
        }

        return everyonePlayed;
    }

    public bool CheckPlayersBet()
    {
        //Did someone Bet this round?
        bool someoneBet = false;

        foreach (var player in PlayerPanels)
        {
            Player p = player.GetComponent<Player>();

            if (player.activeSelf && p != currentPlayer)
            {
                if (p.Bet > 0 && !flop)
                {
                    someoneBet = true;
                    break;
                }
                else if (p.Bet > 0 && flop)
                {
                    someoneBet = true;
                    break;
                }
                else if (p.Bet > 0 && fourth)
                {
                    someoneBet = true;
                    break;
                }
                else if (p.Bet > 0 && fifth)
                {
                    someoneBet = true;
                    break;
                }
            }
        }

        return someoneBet;
    }

    public bool IsTherePlayerPlaying()
    {
        bool isTherePlayersPlaying;
        int totalPlaying = 0;

        foreach (var player in PlayerPanels)
        {
            if (player.activeSelf)
            {
                Player p = player.GetComponent<Player>();

                if (!p.stopPlaying)
                {
                    totalPlaying++;
                }
            }
        }

        if (totalPlaying >= 2)
        {
            isTherePlayersPlaying = true;
        }
        else
        {
            isTherePlayersPlaying = false;
        }

        return isTherePlayersPlaying;
    }

    public bool CheckIfAllPlayersHasSameBet()
    {
        bool sameBet = false;

        foreach (var player1 in PlayerPanels)
        {
            Player p1 = player1.GetComponent<Player>();

            foreach (var player2 in PlayerPanels)
            {
                Player p2 = player2.GetComponent<Player>();

                if (player1.activeSelf && player2.activeSelf)
                {
                    if (p1.TotalBet == p2.TotalBet && p1 != p2 && ((p1.raised && p2.raised) || (p1.raised && p2.called) || (p1.called && p2.raised) || (p1.check && p2.check)))
                    {
                        sameBet = true;
                        break;
                    }
                    else
                    {
                        sameBet = false;
                    }
                }
            }
        }

        return sameBet;
    }

    public int CheckHighestBet()
    {
        int highestBet = 0;

        foreach (var player in PlayerPanels)
        {
            Player p = player.GetComponent<Player>();

            if (p.Bet > highestBet && !flop)
            {
                highestBet = p.Bet;
            }
            if (p.Bet > highestBet && flop)
            {
                highestBet = p.Bet;
            }
            if (p.Bet > highestBet && fourth)
            {
                highestBet = p.Bet;
            }
            if (p.Bet > highestBet && fifth)
            {
                highestBet = p.Bet;
            }
        }

        return highestBet;
    }

    private List<GameObject> CheckPlayersHand()
    {
        List<GameObject> activePlayer = new List<GameObject>();
        List<GameObject> Rank = new List<GameObject>();

        List<string> player1 = new List<string>();
        List<string> player2 = new List<string>();

        foreach (var player in PlayerPanels)
        {
            if (player.activeSelf)
                activePlayer.Add(player);
        }

        for (int i = 0; i < activePlayer.Count; i++)
        {
            Player p = activePlayer[i].GetComponent<Player>();

            if(i == 0)
            {
                player1 = p.Hand;
            }
            else if(i == 1)
            {
                player2 = p.Hand;
            }
        }

        int p1 = CheckRankOfHand(player1);
        int p2 = CheckRankOfHand(player2);

        if (p1 > p2)
        {
            Rank.Add(activePlayer[0]);
            Rank.Add(activePlayer[1]);

            display.SetPokerHand(Rank[0], p1);

            return Rank;
        }
        else if(p2 > p1)
        {
            Rank.Add(activePlayer[1]);
            Rank.Add(activePlayer[0]);

            display.SetPokerHand(Rank[0], p2);

            return Rank;
        }
        else if(p1 == p2) // Highest Pair -> if same Pair else Highest left Card
        {
            GameObject player = PlayerWithHighestPair(activePlayer[0], activePlayer[1]);
            if (player)
            {
                Rank.Add(player);
                display.SetPokerHand(player, 0);
            }
            else
            {
                Rank.Add(HasHighestCard(activePlayer[0], activePlayer[1])[0]);
                display.SetPokerHand(Rank[0], 1);
            }

            return Rank;
        }
        else
        {
            return null;
        }
    }

    private int CheckRankOfHand(List<string> playerHand)
    {
        int rank = 0;

        if (HasRoyalStraightFlush(table_cards, playerHand))
        {
            rank = 10;
            Debug.Log("Has Royal Straight Flush");
        }
        else if (HasStraightFlush(table_cards, playerHand))
        {
            rank = 9;
            Debug.Log("Has Straigh Flush");
        }
        else if (HasFour(table_cards, playerHand))
        {
            rank = 8;
            Debug.Log("Has Four");
        }
        else if (HasFullHouse(table_cards, playerHand))
        {
            rank = 7;
            Debug.Log("Has Full House");
        }
        else if (HasFlush(table_cards, playerHand))
        {
            rank = 6;
            Debug.Log("Has Flush");
        }
        else if (HasStraight(table_cards, playerHand))
        {
            rank = 5;
            Debug.Log("Has Straight");
        }
        else if (HasThree(table_cards, playerHand))
        {
            rank = 4;
            Debug.Log("Has Three");
        }
        else if (HasTwoPairs(table_cards, playerHand))
        {
            rank = 3;
            Debug.Log("Has Two Pairs");
        }
        else if (HasPairs(table_cards, playerHand))
        {
            rank = 2;
            Debug.Log("Has One Pair");
        }

        return rank;
    }

    private List<GameObject> HasHighestCard(GameObject player1, GameObject player2)
    {
        List<GameObject> playerPositions = new List<GameObject>();
        // Create an array of possible ranks for a straight.
        string[] possibleRanks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        int player1Card = -1;
        int player2Card = -1;

        foreach (var playerObject in new GameObject[] { player1, player2 })
        {
            var playerHand = playerObject.GetComponent<Player>().Hand;

            foreach (var card in playerHand)
            {
                string rank = card.Substring(0, 1);

                if (possibleRanks.Contains(rank))
                {
                    int cardIndex = Array.IndexOf(possibleRanks, rank);
                    int currentPlayerCard = playerObject == player1 ? player1Card : player2Card;

                    if (cardIndex > currentPlayerCard)
                    {
                        if (playerObject == player1)
                        {
                            player1Card = cardIndex;
                        }
                        else
                        {
                            player2Card = cardIndex;
                        }
                    }
                }
            }
        }

        if (player1Card > player2Card)
        {
            Debug.Log("Player1 Has Highest Card!");
            playerPositions.Add(player1);
            playerPositions.Add(player2);
            return playerPositions;
        }
        else if (player2Card > player1Card)
        {
            Debug.Log("Player2 Has Highest Card!");
            playerPositions.Add(player2);
            playerPositions.Add(player1);
            return playerPositions;
        }
        else
        {
            Debug.Log("Both Players Have the Same Highest Card!");
            return null; // Return null or handle ties as needed.
        }
    }

    private bool HasPairs(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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
            foreach (string card in playerHand)
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

    private GameObject PlayerWithHighestPair(GameObject player1Hand, GameObject player2Hand)
    {
        List<string> combinedHand = new List<string>(player1Hand.GetComponent<Player>().Hand);
        combinedHand.AddRange(table_cards);

        List<string> combinedHand2 = new List<string>(player2Hand.GetComponent<Player>().Hand);
        combinedHand2.AddRange(table_cards);

        Dictionary<string, int> rankCount1 = new Dictionary<string, int>();
        Dictionary<string, int> rankCount2 = new Dictionary<string, int>();

        foreach (string card in combinedHand)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount1.ContainsKey(rank))
            {
                rankCount1[rank] = 0;
            }

            rankCount1[rank]++;
        }

        foreach (string card in combinedHand2)
        {
            string rank = card.Substring(0, 1);

            if (!rankCount2.ContainsKey(rank))
            {
                rankCount2[rank] = 0;
            }

            rankCount2[rank]++;
        }

        // Find the highest pair in playerHand.
        string highestPairRank1 = null;
        foreach (var kvp in rankCount1)
        {
            if (kvp.Value == 2)
            {
                highestPairRank1 = kvp.Key;
                break;
            }
        }

        // Find the highest pair in player2Hand.
        string highestPairRank2 = null;
        foreach (var kvp in rankCount2)
        {
            if (kvp.Value == 2)
            {
                highestPairRank2 = kvp.Key;
                break;
            }
        }

        if (highestPairRank1 != null && highestPairRank2 != null)
        {
            // Both players have a pair, compare their pair ranks.
            if (highestPairRank1.CompareTo(highestPairRank2) > 0)
            {
                return player1Hand;
            }
            else if (highestPairRank2.CompareTo(highestPairRank1) > 0)
            {
                return player2Hand;
            }
            else
            {
                // The pairs are of the same rank; compare the highest cards not in the pair.
                string highestNonPairCard1 = GetHighestNonPairCard(player1Hand.GetComponent<Player>().Hand, highestPairRank1);
                string highestNonPairCard2 = GetHighestNonPairCard(player2Hand.GetComponent<Player>().Hand, highestPairRank2);

                if (highestNonPairCard1.CompareTo(highestNonPairCard2) > 0)
                {
                    return player1Hand;
                }
                else if (highestNonPairCard2.CompareTo(highestNonPairCard1) > 0)
                {
                    return player2Hand;
                }
                else
                {
                    // Both players have the same pair and the same highest non-pair card; it's a tie.
                    return null;
                }
            }
        }
        else if (highestPairRank1 != null)
        {
            // Only player1 has a pair.
            return player1Hand;
        }
        else if (highestPairRank2 != null)
        {
            // Only player2 has a pair.
            return player2Hand;
        }
        else
        {
            // Neither player has a pair.
            return null;
        }
    }

    private string GetHighestNonPairCard(List<string> hand, string pairRank)
    {
        string highestNonPairCard = null;

        foreach (string card in hand)
        {
            string rank = card.Substring(0, 1);

            if (rank != pairRank && (highestNonPairCard == null || rank.CompareTo(highestNonPairCard) > 0))
            {
                highestNonPairCard = rank;
            }
        }

        return highestNonPairCard;
    }

    private bool HasTwoPairs(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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

        foreach (var card in playerHand)
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

    private bool HasThree(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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
            foreach (string card in playerHand)
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

    private bool HasStraight(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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

    private bool HasFlush(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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

    private bool HasFullHouse(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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
            // Check if at least one card from playerHand is part of the three of a kind or the one pair.
            foreach (string card in playerHand)
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

    private bool HasFour(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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
            foreach (string card in playerHand)
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

    private bool HasStraightFlush(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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

    private bool HasRoyalStraightFlush(List<string> tableCards, List<string> playerHand)
    {
        List<string> combinedHand = new List<string>(playerHand);
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
