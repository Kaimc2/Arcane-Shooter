using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeedManager : MonoBehaviour
{
    public static KillFeedManager Instance;
    public GameObject killMessagePrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddNewMessage(string killer, string how, string victim)
    {
        GameObject message = Instantiate(killMessagePrefab, transform);
        KillMessage killMessage = message.GetComponent<KillMessage>();
        killMessage.SetMessage(killer, how, victim);
    }
}