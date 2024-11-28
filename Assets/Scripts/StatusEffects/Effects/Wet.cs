using UnityEngine;

public class Wet : StatusEffect
{
    public void Initialize(GameObject target, string type, float duration)
    {
        this.target = target;
        this.type = type;
        this.duration = duration;
        elapsedTime = 0f;
    }

    public override void AddEffect()
    {
        Debug.Log($"{target.name} is wet");
        // Instantiate the burn effect
    }

    public override void UpdateEffect()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < duration)
        {
            // Apply movement debuff 
            if (target.CompareTag("Player"))
            {
                // PlayerManager playerManager = target.GetComponent<PlayerManager>();
                // playerManager.TakeDamage(damagePerSecond * Time.deltaTime);
            }
            else if (target.CompareTag("Enemy") || target.CompareTag("Ally"))
            {
                // AIController aiController = target.GetComponent<AIController>();
                // aiController.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            isExpired = true;
            RemoveEffect();
        }
    }

    public override void RemoveEffect()
    {
        Debug.Log($"{target.name} is no longer wet");
        // Clean effect

        Destroy(this);
    }
}