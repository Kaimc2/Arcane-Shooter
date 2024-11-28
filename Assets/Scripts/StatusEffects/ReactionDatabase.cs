using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReactionDatabase : MonoBehaviour
{
    public static Dictionary<string, Action<GameObject, StatusEffect, StatusEffect>> Reactions = new() {
        { "Fire+Water", (target, fire, water) => ExtinguishFire(target, fire, water)}
    };

    public static void ExtinguishFire(GameObject target, StatusEffect fire, StatusEffect water)
    {
        Debug.Log($"{target.name} fire extinguish by water");
        fire.RemoveEffect();
        water.RemoveEffect();
    }
}
