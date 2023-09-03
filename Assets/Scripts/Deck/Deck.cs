using System;
using System.Linq;
using System.Collections.Generic;

public class Deck
{
    private string[] suits = {"S", "C", "H", "D"};
    private string[] cards = {"2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A"};

    public List<string> PokerDeck { get; set; }

    public Deck()
    {
        this.PokerDeck = new List<string>();
        CreateNewDeck();
    }

    private void CreateNewDeck()
    {
        //Adding Cards to the Deck
        foreach (var card in cards)
        {
            foreach (var suit in suits)
            {
                PokerDeck.Add(card + suit);
            }
        }

        //Suffling the Cards
        Random rnd = new Random();
        this.PokerDeck = PokerDeck.OrderBy(item => rnd.Next()).ToList();
    }
}
