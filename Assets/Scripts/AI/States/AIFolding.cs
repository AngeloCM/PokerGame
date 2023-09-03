using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFolding : AIAction
{
    public override AIState Execute(List<string> tableCards, List<string> aiHand, Player player)
    {
        PerformFold(player);
        return AIState.Idle;
    }

    private void PerformFold(Player player)
    {
        Debug.Log(player.Name + " Folded !");
        player.gameObject.GetComponent<UIController>().PokerHand.text = "Folded";


        player.played += 1;
        player.myTurn = false;
        player.stopPlaying = true;

        player.gameObject.GetComponent<UIController>().TurnButtons(false);
    }
}
