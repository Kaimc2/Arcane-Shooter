using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

enum Wizard
{
    Player,
    NPC,
    Enemy
}

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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static float winCondition = 10f;
    private bool isGameOver;

    [Header("Object References")]
    public UIManager uIManager;
    public GameObject[] wizards;
    public List<GameObject> blueTeam;
    public List<GameObject> redTeam;
    public Transform[] blueSpawnPoints;
    public Transform[] redSpawnPoints;

    [Header("Materials")]
    public Material blueTeamMat;
    public Material blueTeamCloakMat;
    public Material redTeamMat;
    public Material redTeamCloakMat;

    // Properties for generating team
    private bool isPlayerTeamBlue;
    private Team playerTeam;
    private Team enemyTeam;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Debug.Log($"The Win Condition is {winCondition}");
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateTeam();
    }

    // Update is called once per frame
    void Update()
    {
        RespawnPlayer();
        RespawnTeam(playerTeam.members, playerTeam.spawnPoints, Wizard.NPC, playerTeam.modelMat, playerTeam.cloakMat);
        RespawnTeam(enemyTeam.members, enemyTeam.spawnPoints, Wizard.Enemy, enemyTeam.modelMat, enemyTeam.cloakMat);
        CheckWinner();
    }

    private void RespawnTeam(List<GameObject> team, Transform[] spawnPoints, Wizard wizardType, Material modelMat, Material cloakMat)
    {
        for (int i = 0; i < team.Count; i++)
        {
            // Respawn team members
            if (team[i] == null)
            {
                team[i] = Instantiate(wizards[(int)wizardType], spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }

        ApplyTeamMat(team, modelMat, cloakMat);
    }

    private void RespawnPlayer()
    {
        if (playerTeam.members[0] == null)
        {
            playerTeam.members[0] = Instantiate(wizards[(int)Wizard.Player], playerTeam.spawnPoints[0].position, playerTeam.spawnPoints[0].rotation);
        }

        // Apply the team texture to player
        Dictionary<string, Material> materialMap = new()
        {
            {"Model", playerTeam.modelMat},
            {"Cloak", playerTeam.cloakMat},
        };
        foreach (Renderer skin in playerTeam.members[0].GetComponentsInChildren<Renderer>())
        {
            if (materialMap.TryGetValue(skin.tag, out Material material))
            {
                skin.material = material;
            }
        }
    }

    private void GenerateTeam()
    {
        // Generate team (0) = Red, (1) = Blue
        isPlayerTeamBlue = Random.Range(0, 2) == 1;
        Debug.Log($"Is Player Team Blue: {isPlayerTeamBlue}");

        // Generate Team
        playerTeam = new(
            isPlayerTeamBlue ? blueTeam : redTeam,
            isPlayerTeamBlue ? blueSpawnPoints : redSpawnPoints,
            isPlayerTeamBlue ? blueTeamMat : redTeamMat,
            isPlayerTeamBlue ? blueTeamCloakMat : redTeamCloakMat,
            isPlayerTeamBlue ? "Blue" : "Red"
        );

        enemyTeam = new(
            isPlayerTeamBlue ? redTeam : blueTeam,
            isPlayerTeamBlue ? redSpawnPoints : blueSpawnPoints,
            isPlayerTeamBlue ? redTeamMat : blueTeamMat,
            isPlayerTeamBlue ? redTeamCloakMat : blueTeamCloakMat,
            isPlayerTeamBlue ? "Red" : "Blue"
        );

        // Add player to their team
        playerTeam.members.Add(Instantiate(wizards[(int)Wizard.Player], playerTeam.spawnPoints[0].position, playerTeam.spawnPoints[0].rotation));

        // Add NPCs to player team
        for (int i = 1; i < playerTeam.spawnPoints.Length; i++)
        {
            playerTeam.members.Add(Instantiate(wizards[(int)Wizard.NPC], playerTeam.spawnPoints[i].position, playerTeam.spawnPoints[i].rotation));
        }

        // Add Enemies to enemy team
        for (int i = 0; i < enemyTeam.spawnPoints.Length; i++)
        {
            enemyTeam.members.Add(Instantiate(wizards[(int)Wizard.Enemy], enemyTeam.spawnPoints[i].position, enemyTeam.spawnPoints[i].rotation));
        }

        // Apply team materials
        ApplyTeamMat(playerTeam.members, playerTeam.modelMat, playerTeam.cloakMat);
        ApplyTeamMat(enemyTeam.members, enemyTeam.modelMat, enemyTeam.cloakMat);
    }

    private void ApplyTeamMat(List<GameObject> team, Material teamMat, Material teamCloakMat)
    {
        Dictionary<string, Material> materialMap = new()
        {
            {"Model", teamMat},
            {"Cloak", teamCloakMat},
        };

        foreach (GameObject wizard in team)
        {
            foreach (Renderer skin in wizard.GetComponentsInChildren<Renderer>())
            {
                if (materialMap.TryGetValue(skin.tag, out Material material))
                {
                    skin.material = material;
                }
            }
        }
    }

    public void UpdateScore(bool isEnemy)
    {
        if (isGameOver) return;

        if (isEnemy)
        {
            playerTeam.score++;
            uIManager.UpdateScore(playerTeam.color, playerTeam.score);
        }
        else
        {
            enemyTeam.score++;
            uIManager.UpdateScore(enemyTeam.color, enemyTeam.score);
        }
    }

    private void CheckWinner()
    {
        if (enemyTeam.score >= winCondition || playerTeam.score >= winCondition)
        {
            // Time.timeScale = 0;
            isGameOver = true;

            if (playerTeam.score > enemyTeam.score)
            {
                uIManager.ToggleGameOverPanel($"{playerTeam.color} Team Win!");
            }
            else
            {
                uIManager.ToggleGameOverPanel($"{enemyTeam.color} Team Win!");
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
