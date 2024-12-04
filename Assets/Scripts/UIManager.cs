using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI manaAmount;
    public TextMeshProUGUI healthAmount;

    public void UpdateMana(float mana)
    {
        manaAmount.text = mana.ToString();
    }

    public void UpdateHealth(float health)
    {
        healthAmount.text = health.ToString();
    }
}
