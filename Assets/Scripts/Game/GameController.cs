using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float resetTimer = 5f;
    public Game game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (game.IsTherePlayerPlaying())
        {
            if (!game.flop && !game.fourth && !game.fifth && !game.finished)
            {
                SetFlopTurn();
            }
            else if (game.flop && !game.fourth && !game.fifth && !game.finished)
            {
                SetFourthTurn();
            }
            else if (game.flop && game.fourth && !game.fifth && !game.finished)
            {
                SetFifthTurn();
            }
            else if (game.flop && game.fourth && game.fifth && !game.finished)
            {
                LastBet();
            }
        }

        if (game.finished)
        {
            resetTimer -= Time.deltaTime;
            game.CheckWinner(); 
            if (resetTimer <= 0)
            {
                SceneManager.LoadScene("PokerGame");
            }
        }
    }

    void SetFlopTurn()
    {
        if (game.CheckIfAllPlayersHasSameBet())
        {
            game.DealFlop();
            game.ResetTurn();
        }
        else
        {
            game.SetNextPlayerTurn();
        }
    }

    void SetFourthTurn()
    {
        if (game.CheckIfAllPlayersHasSameBet())
        {
            game.DealFourthCard();
            game.ResetTurn();
        }
        else
        {
            game.SetNextPlayerTurn();
        }
    }

    void SetFifthTurn()
    {
        if (game.CheckIfAllPlayersHasSameBet())
        {
            game.DealFifthCard();
            game.ResetTurn();
        }
        else
        {
            game.SetNextPlayerTurn();
        }
    }

    void LastBet()
    {
        if (game.CheckIfAllPlayersHasSameBet())
        {
            game.currentPlayer = null;
            game.finished = true;
        }
        else
        {
            game.SetNextPlayerTurn();
        }
    }
}
