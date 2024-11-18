using UnityEngine;

public class LightningEffect : MonoBehaviour
{
  public float knockbackForce = 5f; // Knockback force to apply

  private void OnTriggerEnter(Collider other)
  {
      Rigidbody rb = other.GetComponent<Rigidbody>();
      if (rb != null)
      {
          Debug.Log("Hit object: " + other.name);

          // Calculate knockback direction
          Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

          // Apply force
          rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

          // Optionally set velocity for dramatic movement
          rb.velocity = knockbackDirection * knockbackForce;
      }
  }
}
