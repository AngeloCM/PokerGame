using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILost : AIAction
{
    public override AIState Execute(List<string> tableCards, List<string> aiHand, Player player)
    {
        PerformLost(player);
        return AIState.Idle;
    }

    void PerformLost(Player player)
    {
        Debug.Log(player.Name + " Lost !");

        player.myTurn = false;
        player.stopPlaying = true;

        player.gameObject.GetComponent<UIController>().TurnButtons(false);
    }

}
