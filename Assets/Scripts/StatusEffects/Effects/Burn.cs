using UnityEngine;

public class Burn : StatusEffect
{
    public float damagePerSecond;

    public void Initialize(GameObject target, string type, float duration, float dps)
    {
        this.target = target;
        this.type = type;
        this.duration = duration;
        damagePerSecond = dps;
        elapsedTime = 0f;
    }

    public override void AddEffect()
    {
        Debug.Log($"{target.name} is burning");
        // Instantiate the burn effect
    }

    public override void UpdateEffect()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < duration && !isExpired)
        {
            // Apply damage over time
            if (target.CompareTag("Player"))
            {
                PlayerManager playerManager = target.GetComponent<PlayerManager>();
                playerManager.TakeDamage(damagePerSecond * Time.deltaTime);
            }
            else if (target.CompareTag("Enemy") || target.CompareTag("Ally"))
            {
                AIController aiController = target.GetComponent<AIController>();
                aiController.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            RemoveEffect();
        }
    }

    public override void RemoveEffect()
    {
        isExpired = true;
        Debug.Log($"{target.name} is no longer burning");
        // Clean effect

        Destroy(this);
    }
}