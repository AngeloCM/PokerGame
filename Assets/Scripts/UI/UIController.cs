using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_Text PokerHand;
    [SerializeField] public TMPro.TMP_Text Money;
    [SerializeField] public Slider slider;
    [SerializeField] public TMPro.TMP_InputField inputField;
    [SerializeField] public Button fold;
    [SerializeField] public Button check;
    [SerializeField] public Button call;
    [SerializeField] public Button raise;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnButtons(bool isClickable)
    {
        slider.interactable = isClickable;
        inputField.interactable = isClickable;
        fold.interactable = isClickable;
        check.interactable = isClickable;
        call.interactable = isClickable;
        raise.interactable = isClickable;
    }

    public void TurnFoldButton(bool isClickable)
    {
        fold.interactable = isClickable;
    }

    public void TurnCheckButton(bool isClickable)
    {
        check.interactable = isClickable;
    }

    public void TurnCallButton(bool isClickable)
    {
        call.interactable = isClickable;
    }

    public void TurnRaiseButton(bool isClickable)
    {
        raise.interactable = isClickable;
    }
}
