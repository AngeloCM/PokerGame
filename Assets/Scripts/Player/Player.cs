using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Information")]
    public string Name;
    public int Money = 1000;
    public int TotalMoney;
    public int TotalBet;
    public int Bet;
    public List<string> Hand;

    [Header("Player Status")]
    public bool isAI = false;
    public bool myTurn = false;
    public int played = 0;
    public bool check = false;
    public bool raised = false;
    public bool called = false;
    public bool stopPlaying = false;

    [Header("Game Script")]
    public Game game;

    void Awake()
    {
        this.Money = 1000;
        this.Hand = new List<string>();
    }

    void Start()
    {
        myTurn = false;
        played = 0;
        check = false;
        raised = false;
        called = false;
    }

    void Update()
    {
        if (myTurn)
        {
            CanPlayerCheck();
            CanPlayerRaise();
            CanPlayerCall();

            if (game.CheckPlayersBet() && this.played > 1)
            {
                int highest = game.CheckHighestBet();
                gameObject.GetComponent<UIController>().slider.minValue = highest;
            }
        }
    }

    public void addCardToList(string card)
    {
        this.Hand.Add(card);
    }

    public void Fold()
    {
        Debug.Log(Name + " Folded !");

        played += 1;
        myTurn = false;
        stopPlaying = true;

        gameObject.GetComponent<UIController>().TurnButtons(false);
        // Discart hand. 
        // Stop playing.
    }

    public void Check()
    {
        Debug.Log(Name + " Checked !");

        played += 1;
        myTurn = false;
        check = true;

        gameObject.GetComponent<UIController>().TurnButtons(false);
    }

    public void Call()
    {
        Debug.Log(Name + " Called !");

        played += 1;
        myTurn = false;
        called = true;

        //Check Highest Bet and set as your Bet;
        int highest = game.CheckHighestBet();

        Bet = highest;
        TotalBet += Bet;
        Money -= Bet;
        game.totalBet += Bet;

        gameObject.GetComponent<UIController>().slider.maxValue = Money;
        gameObject.GetComponent<UIController>().Money.text = "$" + Money.ToString();
        gameObject.GetComponent<UIController>().TurnButtons(false);
    }

    public void Raise()
    {
        Debug.Log(Name + " Raised !");

        played += 1;
        myTurn = false;
        raised = true;

        Bet = (int)gameObject.GetComponent<UIController>().slider.value;
        TotalBet += Bet;
        Money -= Bet;
        game.totalBet += Bet;

        gameObject.GetComponent<UIController>().slider.maxValue = Money;
        gameObject.GetComponent<UIController>().Money.text = "$" + Money.ToString();
        gameObject.GetComponent<UIController>().TurnButtons(false);
    }

    public bool CanPlayerCheck()
    {
        if (game.CheckPlayersBet())
        {
            gameObject.GetComponent<UIController>().TurnCheckButton(false);
            return false;
        }
        else
        {
            gameObject.GetComponent<UIController>().TurnCheckButton(true);
            return true;
        }
    }

    public bool CanPlayerRaise()
    {
        if (game.CheckPlayersBet())
        {
            gameObject.GetComponent<UIController>().TurnRaiseButton(false);
            return false;
        }
        else
        {
            gameObject.GetComponent<UIController>().TurnRaiseButton(true);
            return true;
        }
    }

    public bool CanPlayerCall()
    {
        int highest = game.CheckHighestBet();

        if ((highest <= Money + TotalBet) && game.CheckPlayersBet())
        {
            gameObject.GetComponent<UIController>().TurnCallButton(true);
            return true;
        }
        else
        {
            gameObject.GetComponent<UIController>().TurnCallButton(false);
            return false;
        }
    }
}
