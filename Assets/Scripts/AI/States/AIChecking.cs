using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChecking : AIAction
{
    public override AIState Execute(List<string> tableCards, List<string> aiHand, Player player)
    {
        PerformCheck(player);
        return AIState.Idle;
    }

    private void PerformCheck(Player player)
    {
        Debug.Log(player.Name + " Checked !");
        player.gameObject.GetComponent<UIController>().PokerHand.text = "Checked";

        player.played += 1;
        player.myTurn = false;
        player.check = true;

        player.gameObject.GetComponent<UIController>().TurnButtons(false);
    }
}
