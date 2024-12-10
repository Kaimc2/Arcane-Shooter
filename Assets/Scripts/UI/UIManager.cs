using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputActionReference menuInput;

    [Header("Player Stats")]
    public TextMeshProUGUI healthAmount;
    public TextMeshProUGUI manaAmount;

    [Header("Score")]
    public TextMeshProUGUI blueScoreUI;
    public TextMeshProUGUI redScoreUI;

    [Header("Panels")]
    public Transform gameOverPanel;
    public TextMeshProUGUI gameOverMessage;
    public Transform menuPanel;
    public Toggle audioToggle;
    private bool isMenuOpened;
    private readonly bool muted;

    public void UpdateMana(float mana)
    {
        manaAmount.text = mana.ToString();
    }

    public void UpdateHealth(float health)
    {
        healthAmount.text = health.ToString();
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

    public void ToggleMenu()
    {
        if (menuInput.action.IsPressed())
        {
            Cursor.lockState = CursorLockMode.None;
            menuPanel.gameObject.SetActive(!isMenuOpened);
        }
    }

    public void MuteGame()
    {
        AudioListener.pause = !muted;
    }
}
