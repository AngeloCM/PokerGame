using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private AIAction currentAction;
    private AIState currentState = AIState.Idle;

    [Header("AI Player")]
    public Player player;

    private List<string> TableCards;

    private void Start()
    {
        TableCards = player.game.table_cards;
    }

    private void Update()
    {
        if(player.isAI && player.myTurn && !player.stopPlaying)
        {
            switch (currentState)
            {
                case AIState.Idle:
                    currentState = AIState.Deciding;
                    currentAction = new AIDeciding();                    
                    break;

                case AIState.Deciding:
                    currentState = currentAction.Execute(TableCards, player.Hand, player);
                    break;

                case AIState.Checking:
                    currentState = new AIChecking().Execute(TableCards, player.Hand, player);
                    break;

                case AIState.Folding:
                    currentState = new AIFolding().Execute(TableCards, player.Hand, player);
                    break;

                case AIState.Raising:
                    currentState = new AIRaising().Execute(TableCards, player.Hand, player);
                    break;

                case AIState.Calling:
                    currentState = new AICalling().Execute(TableCards, player.Hand, player);
                    break;

                case AIState.Lost:
                    currentState = new AICalling().Execute(TableCards, player.Hand, player);
                    break;
            }
        }
    }
}
