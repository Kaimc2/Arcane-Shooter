using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
  [SerializeField] private Transform player;
  [SerializeField] private float offsetX, offsetZ;
  [SerializeField] private float lerpSpeed;

  public void LateUpdate() {
    Vector3 targetPosition = new Vector3(player.position.x + offsetX, transform.position.y, player.position.z + offsetZ);
    transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
  }
}