using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRaising : AIAction
{
    public override AIState Execute(List<string> tableCards, List<string> aiHand, Player player)
    {
        PerformRaise(player);
        return AIState.Idle;
    }

    private void PerformRaise(Player player) 
    {
        Debug.Log(player.Name + " Raised !");
        player.gameObject.GetComponent<UIController>().PokerHand.text = "Raised";


        int highest = player.game.CheckHighestBet();
        int randomInt;

        if (highest >= player.Money)
        {
            randomInt = Random.Range(player.Money, player.Money);
        }
        else
        {
            randomInt = Random.Range(highest, player.Money);
        }

        player.played += 1;
        player.myTurn = false;
        player.raised = true;

        player.Bet = randomInt;
        player.TotalBet += player.Bet;
        player.Money -= player.Bet;
        player.game.totalBet += player.Bet;

        player.gameObject.GetComponent<UIController>().slider.maxValue = player.Money;
        player.gameObject.GetComponent<UIController>().Money.text = "$" + player.Money.ToString();
        player.gameObject.GetComponent<UIController>().TurnButtons(false);
    }
}
