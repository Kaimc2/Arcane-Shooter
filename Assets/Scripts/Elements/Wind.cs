using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wind : Projectile
{
    public float upwardForce = 10f;
    public float duration = 5f;
    private float _startTime = 0f;

    void Start()
    {
        _startTime = Time.time;
    }

    void Update()
    {
        if (Time.time > duration + _startTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null) playerController.ApplyJumpForce(upwardForce);
        }
        else if (other.gameObject.CompareTag("NPC") || other.gameObject.CompareTag("Enemy"))
        {
            AIController agent = other.GetComponent<AIController>();
            if (agent != null) agent.Jump(upwardForce * 60);
        }
    }
}
