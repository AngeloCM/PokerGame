using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanels(List<GameObject> PlayerPanels, int numPlayers, int numAIs)
    {
        foreach (var player in PlayerPanels)
        {
            if (!player.activeSelf && numPlayers > 0)
            {
                player.SetActive(true);
                numPlayers--;
            }
            else if (!player.activeSelf && numAIs > 0)
            {
                player.SetActive(true);
                numAIs--;
            }
        }
    }

    public void SetPlayerPanel(List<GameObject> PlayerPanels)
    {
        foreach (var player in PlayerPanels)
        {
            if (player.activeSelf)
            {
                string name = player.GetComponent<Player>().Name;
                string card1 = player.GetComponent<Player>().Hand[0];
                string card2 = player.GetComponent<Player>().Hand[1];
                string money = player.GetComponent<Player>().Money.ToString();

                foreach (var transform in player.GetComponentsInChildren<Transform>())
                {
                    if (transform.name == "Name")
                    {
                        transform.GetComponent<TMPro.TextMeshProUGUI>().text = name;
                    }

                    if(transform.name == "Money")
                    {
                        transform.GetComponent<TMPro.TextMeshProUGUI>().text = " " + money;
                    }

                    if(transform.name == "Card1")
                    {
                        Sprite s = Resources.Load<Sprite>("Sprites/Cards/" + card1);
                        transform.GetComponent<Image>().sprite = s;
                    }

                    if (transform.name == "Card2")
                    {
                        Sprite s = Resources.Load<Sprite>("Sprites/Cards/" + card2);
                        transform.GetComponent<Image>().sprite = s;
                    }
                }
            }
        }
    }

    public void SetCardInFlop(GameObject TablePanel, List<string> FlopCards)
    {
        foreach (var transform in TablePanel.GetComponentsInChildren<Transform>())
        {
            
            if (transform.name == "Card1")
            {
                Sprite s = Resources.Load<Sprite>("Sprites/Cards/" + FlopCards[0]);
                transform.GetComponent<Image>().sprite = s;
            }

            if (transform.name == "Card2")
            {
                Sprite s = Resources.Load<Sprite>("Sprites/Cards/" + FlopCards[1]);
                transform.GetComponent<Image>().sprite = s;
            }

            if (transform.name == "Card3")
            {
                Sprite s = Resources.Load<Sprite>("Sprites/Cards/" + FlopCards[2]);
                transform.GetComponent<Image>().sprite = s;
            }
            
        }
    }

    public void SetFourthCard(GameObject TablePanel, string FourthCard)
    {
        foreach (var transform in TablePanel.GetComponentsInChildren<Transform>())
        {
            if (transform.name == "Card4")
            {
                Sprite s = Resources.Load<Sprite>("Sprites/Cards/" + FourthCard);
                transform.GetComponent<Image>().sprite = s;
            }
        }
    }

    public void SetFifthCard(GameObject TablePanel, string FifthCard)
    {
        foreach (var transform in TablePanel.GetComponentsInChildren<Transform>())
        {
            if (transform.name == "Card5")
            {
                Sprite s = Resources.Load<Sprite>("Sprites/Cards/" + FifthCard);
                transform.GetComponent<Image>().sprite = s;
            }
        }
    }

    public void SetPokerHand(GameObject player , int playerRank)
    {
        if (playerRank == 0)
        {
            player.GetComponent<UIController>().PokerHand.text = "Highest Pair";
        }
        else if (playerRank == 1)
        {
            player.GetComponent<UIController>().PokerHand.text = "Highest Card";
        }
        else if (playerRank == 2)
        {
            player.GetComponent<UIController>().PokerHand.text = "One Pair";
        }
        else if (playerRank == 3)
        {
            player.GetComponent<UIController>().PokerHand.text = "Two Pairs";
        }
        else if (playerRank == 4)
        {
            player.GetComponent<UIController>().PokerHand.text = "Three";
        }
        else if (playerRank == 5)
        {
            player.GetComponent<UIController>().PokerHand.text = "Straight";
        }
        else if (playerRank == 6)
        {
            player.GetComponent<UIController>().PokerHand.text = "Flush";
        }
        else if (playerRank == 7)
        {
            player.GetComponent<UIController>().PokerHand.text = "Full House";
        }
        else if (playerRank == 8)
        {
            player.GetComponent<UIController>().PokerHand.text = "Four";
        }
        else if (playerRank == 9)
        {
            player.GetComponent<UIController>().PokerHand.text = "Straigh Flush";
        }
        else if (playerRank == 10)
        {
            player.GetComponent<UIController>().PokerHand.text = "Royal Straight Flush";
        }
    }
}
