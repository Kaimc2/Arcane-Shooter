using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new();

    public List<StatusEffect> GetActiveEffects()
    {
        return activeEffects;
    }

    public void AddEffect(StatusEffect newEffect)
    {
        bool alreadyActive = false;
        foreach (StatusEffect activeEffect in activeEffects)
        {
            if (activeEffect.type == newEffect.type)
            {
                Debug.Log("Re-apply effect");
                alreadyActive = true;
                activeEffect.ResetEffect();
                break;
            }
        }

        if (!alreadyActive)
        {
            activeEffects.Add(newEffect);
            newEffect.AddEffect();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].UpdateEffect();
            if (activeEffects[i].isExpired)
            {
                activeEffects[i].RemoveEffect();
                activeEffects.RemoveAt(i);
            }
        }
    }
}
