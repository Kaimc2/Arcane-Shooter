using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Wizard
{
    Player,
    NPC,
    Enemy
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static float winCondition = 10f;
    private bool isGameOver;

    [Header("Object References")]
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
    public bool isPlayerTeamBlue;
    private Team playerTeam;
    private List<string> playerTeamNames;
    private Team enemyTeam;
    private List<string> enemyTeamNames;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        if (SceneManager.GetActiveScene().buildIndex != 1) CheckWinner();
    }

    private void RespawnTeam(List<GameObject> team, Transform[] spawnPoints, Wizard wizardType, Material modelMat, Material cloakMat)
    {
        for (int i = 0; i < team.Count; i++)
        {
            // Respawn team members
            if (team[i] == null)
            {
                GameObject aiObj = Instantiate(wizards[(int)wizardType], spawnPoints[i].position, spawnPoints[i].rotation);
                if (wizardType == Wizard.NPC) aiObj.name = playerTeamNames[i];
                if (wizardType == Wizard.Enemy) aiObj.name = enemyTeamNames[i];
                team[i] = aiObj;
            }
        }

        ApplyTeamMat(team, modelMat, cloakMat);
    }

    private void RespawnPlayer()
    {
        if (playerTeam.members[0] == null)
        {
            UIManager.Instance.GameHub.SetActive(true);
            GameObject playerObj = Instantiate(wizards[(int)Wizard.Player], playerTeam.spawnPoints[0].position, playerTeam.spawnPoints[0].rotation); 
            playerObj.name = "Player";
            playerTeam.members[0] = playerObj;
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
        NameList nameList = new();

        // Generate team (0) = Red, (1) = Blue
        isPlayerTeamBlue = Random.Range(0, 2) == 1;

        // Generate Team
        playerTeam = new(
            isPlayerTeamBlue ? blueTeam : redTeam,
            isPlayerTeamBlue ? blueSpawnPoints : redSpawnPoints,
            isPlayerTeamBlue ? blueTeamMat : redTeamMat,
            isPlayerTeamBlue ? blueTeamCloakMat : redTeamCloakMat,
            isPlayerTeamBlue ? "Blue" : "Red"
        );
        playerTeamNames = new();

        enemyTeam = new(
            isPlayerTeamBlue ? redTeam : blueTeam,
            isPlayerTeamBlue ? redSpawnPoints : blueSpawnPoints,
            isPlayerTeamBlue ? redTeamMat : blueTeamMat,
            isPlayerTeamBlue ? redTeamCloakMat : blueTeamCloakMat,
            isPlayerTeamBlue ? "Red" : "Blue"
        );
        enemyTeamNames = new();

        // Set the score background
        UIManager.Instance.playerTeamScoreBG.color = isPlayerTeamBlue ? Color.blue : Color.red;
        UIManager.Instance.enemyTeamScoreBG.color = isPlayerTeamBlue ? Color.red : Color.blue;

        // Add player to their team
        UIManager.Instance.GameHub.SetActive(true);
        GameObject playerObj = Instantiate(wizards[(int)Wizard.Player], playerTeam.spawnPoints[0].position, playerTeam.spawnPoints[0].rotation);
        playerObj.name = "Player";
        playerTeam.members.Add(playerObj);
        playerTeamNames.Add(playerObj.name);

        // Add NPCs to player team
        for (int i = 1; i < playerTeam.spawnPoints.Length; i++)
        {
            GameObject npc = Instantiate(wizards[(int)Wizard.NPC], playerTeam.spawnPoints[i].position, playerTeam.spawnPoints[i].rotation);
            npc.name = $"{nameList.GetRandomName()} (NPC)";
            playerTeam.members.Add(npc);
            playerTeamNames.Add(npc.name);
        }

        // Add Enemies to enemy team
        for (int i = 0; i < enemyTeam.spawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(wizards[(int)Wizard.Enemy], enemyTeam.spawnPoints[i].position, enemyTeam.spawnPoints[i].rotation);
            enemy.name = $"{nameList.GetRandomName()} (Enemy)";
            enemyTeam.members.Add(enemy);
            enemyTeamNames.Add(enemy.name);
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
            UIManager.Instance.UpdateScore("Player", playerTeam.score);
        }
        else
        {
            enemyTeam.score++;
            UIManager.Instance.UpdateScore("Enemy", enemyTeam.score);
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
                UIManager.Instance.ToggleGameOverPanel($"{playerTeam.color} Team Win!");
            }
            else
            {
                UIManager.Instance.ToggleGameOverPanel($"{enemyTeam.color} Team Win!");
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
