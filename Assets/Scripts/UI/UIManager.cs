using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputActionAsset inputActions;
    public InputActionReference menuAction;
    private InputActionMap gameplayMap;
    private InputActionMap uiMap;

    [Header("Player Stats")]
    public Image healthAmount;
    public Image staminaAmount;
    public Image manaAmount;
    public Image currentWeaponImage;
    public TextMeshProUGUI currentWeapon;
    public Image cooldownOverlay;

    [Header("Score")]
    public TextMeshProUGUI blueScoreUI;
    public TextMeshProUGUI redScoreUI;

    [Header("Panel References")]
    public Transform actionPanel;
    public Transform levelSelectPanel;
    public Transform settingsPanel;
    public Transform gameOverPanel;
    public GameObject GameHub;

    [Header("Panel Properties")]
    public TextMeshProUGUI gameOverMessage;
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityAmt;

    private bool isSettingsOpen = false;

    void Start()
    {
        if (inputActions != null)
        {
            gameplayMap = inputActions.FindActionMap("Player");
            uiMap = inputActions.FindActionMap("UI");

            gameplayMap.Enable();
            uiMap.Enable();
        }

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
        healthAmount.fillAmount = Mathf.Clamp01(health / maxHealth);
    }

    public void UpdateStamina(float stamina, float maxStamina)
    {
        staminaAmount.fillAmount = Mathf.Clamp01(stamina / maxStamina);
    }

    public void UpdateWeaponUI(string weaponName)
    {
        currentWeapon.text = string.Join(" ", weaponName.Split('_'));
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
            case "Blue":
                blueScoreUI.text = score.ToString();
                break;
            case "Red":
                redScoreUI.text = score.ToString();
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
    }
    public void ToggleSettings()
    {
        if (menuAction == null) return;

        if (menuAction.action.WasPressedThisFrame())
        {
            Debug.Log("Open Settings");
            if (!isSettingsOpen)
            {
                gameplayMap.Disable();
                PlayerAnimator.active = false;
            }
            else
            {
                gameplayMap.Enable();
                PlayerAnimator.active = true;
            }
            settingsPanel.gameObject.SetActive(!isSettingsOpen);

            isSettingsOpen = !isSettingsOpen;
            Cursor.lockState = isSettingsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void ChangeSensitivity()
    {
        CameraController.mouseSensitivity = sensitivitySlider.value;
        sensitivityAmt.text = Mathf.Round(sensitivitySlider.value).ToString();
    }
}
