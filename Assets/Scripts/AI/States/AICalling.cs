using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICalling : AIAction
{
    public override AIState Execute(List<string> tableCards, List<string> aiHand, Player player)
    {
        PerformCall(player);
        return AIState.Idle;
    }

    private void PerformCall(Player player) 
    {
        Debug.Log(player.Name + " Called !");
        player.gameObject.GetComponent<UIController>().PokerHand.text = "Called";


        player.played += 1;
        player.myTurn = false;
        player.called = true;

        //Check Highest Bet and set as your Bet;
        int highest = player.game.CheckHighestBet();

        player.Bet = highest;
        player.TotalBet += player.Bet;
        player.Money -= player.Bet;
        player.game.totalBet += player.Bet;

        player.gameObject.GetComponent<UIController>().slider.maxValue = player.Money;
        player.gameObject.GetComponent<UIController>().Money.text = "$" + player.Money.ToString();
        player.gameObject.GetComponent<UIController>().TurnButtons(false);
        // Increment same amount of money as the other player.
        // Next Player turn.
    }
}
