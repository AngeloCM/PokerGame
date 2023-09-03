using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInput : MonoBehaviour
{
    public Player p;
    public Slider slider;
    public TMPro.TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = p.Money;
    }

    // Update is called once per frame
    void Update()
    {
        inputField.text = slider.value.ToString();
    }
}
