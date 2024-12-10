using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    public static void ApplyEffect(GameObject target, StatusEffect newEffect)
    {
        // Grab the active effects on the target
        EffectManager effectManager = target.GetComponent<EffectManager>();
        List<StatusEffect> activeEffects = effectManager.GetActiveEffects();

        foreach (StatusEffect activeEffect in activeEffects)
        {
            // Create the reaction key. E.g. Fire+Water
            string reactionKey = ReactionDatabase.NormalizeKey(activeEffect.type, newEffect.type);
            Debug.Log($"Reaction key: {reactionKey}");

            // Check for reaction from the database
            if (ReactionDatabase.Instance.Reactions.TryGetValue(reactionKey, out Action<GameObject, StatusEffect, StatusEffect> reactionAction))
            {
                reactionAction.Invoke(target, activeEffect, newEffect);
                return; // Stop further action when a reaction occurs
            }
        }

        // No reaction, add the effect to the target
        effectManager.AddEffect(newEffect);
    }
}
