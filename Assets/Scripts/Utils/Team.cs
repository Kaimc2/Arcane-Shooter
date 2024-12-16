using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public List<GameObject> members;
    public Transform[] spawnPoints;
    public Material modelMat;
    public Material cloakMat;
    public float score = 0f;
    public string color;

    public Team(List<GameObject> members, Transform[] spawnPoints, Material material, Material cloakMat, string color)
    {
        this.members = members;
        this.spawnPoints = spawnPoints;
        modelMat = material;
        this.cloakMat = cloakMat;
        this.color = color;
    }
}
