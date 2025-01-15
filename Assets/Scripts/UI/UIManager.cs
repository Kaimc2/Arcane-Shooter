using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public InputActionAsset inputActions;
    public InputActionReference menuAction;
    private InputActionMap gameplayMap;
    private InputActionMap uiMap;
    private float countdown;

    [Header("Player Stats")]
    public Image healthAmount;
    public Image staminaAmount;
    public Image manaAmount;
    public Image currentWeaponImage;
    public TextMeshProUGUI currentWeapon;
    public Image cooldownOverlay;

    [Header("Score")]
    public TextMeshProUGUI playerTeamScoreUI;
    public Image playerTeamScoreBG;
    public TextMeshProUGUI enemyTeamScoreUI;
    public Image enemyTeamScoreBG;

    [Header("Panel References")]
    public Transform actionPanel;
    public Transform levelSelectPanel;
    public Transform settingsPanel;
    public Transform gameOverPanel;
    public GameObject countdownPanel;
    public GameObject GameHub;

    [Header("Panel Properties")]
    public TextMeshProUGUI gameOverMessage;
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityAmt;
    public TextMeshProUGUI countdownText;

    private bool isSettingsOpen = false;

    void Awake()
    {
        // Implement Singleton concept
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (inputActions != null)
        {
            gameplayMap = inputActions.FindActionMap("Player");
            uiMap = inputActions.FindActionMap("UI");

            gameplayMap.Enable();
            uiMap.Enable();
        }

        CameraController.mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 80f);
        sensitivityAmt.text = Mathf.Round(CameraController.mouseSensitivity).ToString();
        sensitivitySlider.value = CameraController.mouseSensitivity;
    }

    void Update()
    {
        ToggleSettings();
    }

    public void UpdateMana(float mana, float maxMana)
    {
        manaAmount.fillAmount = Mathf.Clamp01(mana / maxMana);
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        // healthAmount.fillAmount = Mathf.Clamp01(health / maxHealth);
        float healthPercentage = Mathf.Clamp01(health / maxHealth);

        // Update the health bar fill amount
        healthAmount.fillAmount = healthPercentage;

        // Change the health bar color based on the health percentage
        if (healthPercentage < 0.30f)
        {
            healthAmount.color = Color.red;
        }
        else if (healthPercentage < 0.65f)
        {
            healthAmount.color = Color.yellow;
        }
        else
        {
            healthAmount.color = Color.green;
        }
    }

    public void UpdateStamina(float stamina, float maxStamina)
    {
        staminaAmount.fillAmount = Mathf.Clamp01(stamina / maxStamina);
    }

    public void UpdateWeaponUI(string weaponName)
    {
        currentWeapon.text = string.Join(" ", weaponName.Split('_'));
        currentWeaponImage.sprite = Resources.Load<Sprite>("Images/Staffs/" + weaponName.Split('_')[0]);
    }

    public void UpdateCooldownOverlay(float duration, float maxDuration)
    {
        cooldownOverlay.fillAmount = Mathf.Clamp01(duration / maxDuration);
    }

    public void ToggleGameOverPanel(string message)
    {
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.gameObject.SetActive(true);
        gameOverMessage.text = message;
    }

    public void UpdateScore(string team, float score)
    {
        switch (team)
        {
            case "Player":
                playerTeamScoreUI.text = score.ToString();
                break;
            case "Enemy":
                enemyTeamScoreUI.text = score.ToString();
                break;
            default:
                break;
        }
    }

    public void OpenLevelSelect()
    {
        levelSelectPanel.gameObject.SetActive(true);
        actionPanel.gameObject.SetActive(false);
    }
    public void CloseLevelSelect()
    {
        actionPanel.gameObject.SetActive(true);
        levelSelectPanel.gameObject.SetActive(false);
    }

    public void OpenSettings()
    {
        isSettingsOpen = true;
        settingsPanel.gameObject.SetActive(true);
        if (actionPanel != null) actionPanel.gameObject.SetActive(false);

        if (gameplayMap != null)
        {
            Cursor.lockState = CursorLockMode.None;
            gameplayMap.Disable();
            PlayerAnimator.active = false;
        }
    }
    public void CloseSettings()
    {
        isSettingsOpen = false;
        if (actionPanel != null) actionPanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);

        if (gameplayMap != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            gameplayMap.Enable();
            PlayerAnimator.active = true;
        }

        PlayerPrefs.SetFloat("MouseSensitivity", CameraController.mouseSensitivity);
        PlayerPrefs.Save();
    }
    public void ToggleSettings()
    {
        if (menuAction == null) return;

        if (menuAction.action.WasPressedThisFrame())
        {
            if (!isSettingsOpen)
            {
                gameplayMap.Disable();
                // Don't mess with animation when on title scene
                if (SceneManager.GetActiveScene().buildIndex != 0) PlayerAnimator.active = false;
            }
            else
            {
                gameplayMap.Enable();
                if (SceneManager.GetActiveScene().buildIndex != 0) PlayerAnimator.active = true;
                PlayerPrefs.SetFloat("MouseSensitivity", CameraController.mouseSensitivity);
                PlayerPrefs.Save();
            }
            settingsPanel.gameObject.SetActive(!isSettingsOpen);

            isSettingsOpen = !isSettingsOpen;
            Cursor.lockState = isSettingsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void StartRespawnCountdown()
    {
        // Set set default countdown value
        countdown = 5;
        countdownText.text = Mathf.Round(countdown).ToString();
        countdownPanel.SetActive(true);
        Debug.Log("Start Respawn");

        StartCoroutine(CountdownRoutine());
    }
    private IEnumerator CountdownRoutine()
    {
        while (countdown > 0)
        {
            countdown -= 1f * Time.deltaTime;
            countdownText.text = Mathf.Round(countdown).ToString();
            yield return null;
        }

        countdownPanel.SetActive(false);
    }

    public void ChangeSensitivity()
    {
        CameraController.mouseSensitivity = sensitivitySlider.value;
        sensitivityAmt.text = Mathf.Round(sensitivitySlider.value).ToString();
    }

}
