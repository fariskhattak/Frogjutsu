using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
        Debug.Log("Set mana bar max value to: " + slider.maxValue);
        Debug.Log("Current mana bar value: " + slider.value);
    }

    public void SetMana(int mana)
    {
        slider.value = mana;
    }
}
