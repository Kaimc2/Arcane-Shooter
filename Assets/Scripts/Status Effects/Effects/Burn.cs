using UnityEngine;

public class Burn : StatusEffect
{
    private float damagePerSecond;
    private GameObject burnVFX;
    private GameObject burnObject;

    public void Initialize(GameObject target, GameObject burnVFX, string type, float duration, float dps)
    {
        this.target = target;
        this.type = type;
        this.duration = duration;
        this.burnVFX = burnVFX;
        damagePerSecond = dps;
        elapsedTime = 0f;
    }

    public override void AddEffect()
    {
        // Debug.Log($"{target.name} is burning");

        // Instantiate the burn effect
        burnObject = Instantiate(burnVFX, target.transform.position, target.transform.rotation);
    }

    public override void UpdateEffect()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < duration && !isExpired)
        {
            burnObject.transform.position = target.transform.position;

            // Apply damage over time
            if (target.CompareTag("Player"))
            {
                PlayerManager playerManager = target.GetComponent<PlayerManager>();
                playerManager.TakeDamage(damagePerSecond * Time.deltaTime);
            }
            else if (target.CompareTag("Enemy") || target.CompareTag("NPC"))
            {
                AIController aiController = target.GetComponent<AIController>();
                aiController.TakeDamage(damagePerSecond * Time.deltaTime, "burned to death", "");
            }
        }
        else
        {
            Destroy(burnObject);
            RemoveEffect();
        }
    }

    public override void RemoveEffect()
    {
        isExpired = true;
        // Debug.Log($"{target.name} is no longer burning");

        // Clean effect
        Destroy(this);
    }
}