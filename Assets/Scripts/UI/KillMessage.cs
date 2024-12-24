using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum KillMesssageType {
    Default,
    StatusEffect
}

public class KillMessage : MonoBehaviour
{
    public TextMeshProUGUI killerDisplay;
    public TextMeshProUGUI howDisplay;
    public TextMeshProUGUI victimDisplay;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);        
    }

    public void SetMessage(string killer, string how, string victim)
    {
        killerDisplay.text = killer;
        howDisplay.text = how;
        victimDisplay.text = victim;
        if (victim.Contains("(Enemy)"))
        {
            killerDisplay.color = Color.blue; 
            howDisplay.color = Color.blue;
            victimDisplay.color = Color.blue;
        }
        else
        {
            killerDisplay.color = Color.red; 
            howDisplay.color = Color.red;
            victimDisplay.color = Color.red;
        }
    }
}
