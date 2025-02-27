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
            killerDisplay.color = GameManager.Instance.isPlayerTeamBlue ? Color.blue : Color.red; 
            howDisplay.color = GameManager.Instance.isPlayerTeamBlue ? Color.blue : Color.red;
            victimDisplay.color = GameManager.Instance.isPlayerTeamBlue ? Color.blue : Color.red;
        }
        else
        {
            killerDisplay.color = GameManager.Instance.isPlayerTeamBlue ? Color.red : Color.blue; 
            howDisplay.color = GameManager.Instance.isPlayerTeamBlue ? Color.red : Color.blue;
            victimDisplay.color = GameManager.Instance.isPlayerTeamBlue ? Color.red : Color.blue;
        }
    }
}
