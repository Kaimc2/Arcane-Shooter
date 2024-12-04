using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Wizard
{
    Player,
    NPC,
    Enemy
}

public class GameManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        GenerateTeam();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Respawn()
    {

    }

    private void GenerateTeam()
    {
        Debug.Log("Generating team");

        // Generate team (0) = Red, (1) = Blue
        bool isPlayerTeamBlue = Random.Range(0, 2) == 1;
        Debug.Log($"Is Player Team Blue: {isPlayerTeamBlue}");

        // Generate Team
        List<GameObject> playerTeam = isPlayerTeamBlue ? blueTeam : redTeam;
        List<GameObject> enemyTeam = isPlayerTeamBlue ? redTeam : blueTeam;

        Transform[] playerSpawnPoints = isPlayerTeamBlue ? blueSpawnPoints : redSpawnPoints;
        Transform[] enemySpawnPoints = isPlayerTeamBlue ? redSpawnPoints : blueSpawnPoints;

        Material playerTeamMat = isPlayerTeamBlue ? blueTeamMat : redTeamMat;
        Material enemyTeamMat = isPlayerTeamBlue ? redTeamMat : blueTeamMat;

        Material playerTeamCloakMat = isPlayerTeamBlue ? blueTeamCloakMat : redTeamCloakMat;
        Material enemyTeamCloakMat = isPlayerTeamBlue ? redTeamCloakMat : blueTeamCloakMat;


        // Add player to their team
        playerTeam.Add(Instantiate(wizards[(int)Wizard.Player], playerSpawnPoints[0].position, playerSpawnPoints[0].rotation));

        // Add NPCs to player team
        for (int i = 1; i < playerSpawnPoints.Length; i++)
        {
            playerTeam.Add(Instantiate(wizards[(int)Wizard.NPC], playerSpawnPoints[i].position, playerSpawnPoints[i].rotation));
        }

        // Add Enemies to enemy team
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            enemyTeam.Add(Instantiate(wizards[(int)Wizard.Enemy], enemySpawnPoints[i].position, enemySpawnPoints[i].rotation));
        }

        // Apply team materials
        ApplyTeamMat(playerTeam, playerTeamMat, playerTeamCloakMat);
        ApplyTeamMat(enemyTeam, enemyTeamMat, enemyTeamCloakMat);
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
}
