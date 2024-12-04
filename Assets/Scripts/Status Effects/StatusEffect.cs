using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
   public string type;  // E.g. Fire, Water, Electric
   public float duration;
   public float elapsedTime;
   public bool isExpired;
   protected GameObject target;

    public abstract void AddEffect();
   public abstract void UpdateEffect();
   public abstract void RemoveEffect();
   public virtual void ResetEffect()
   {
      elapsedTime = 0;
   }
}