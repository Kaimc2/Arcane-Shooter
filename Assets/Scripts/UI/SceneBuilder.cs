using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneBuilder : MonoBehaviour
{
    public TMP_Dropdown levelDropdown;
    public Sprite[] levelImages;
    public Image currentLevelImg;
    public GameObject winConditionOptions;
    public TextMeshProUGUI winAmountUI;

    [Header("Loading Screen")]
    public GameObject loaderCanvas;
    public Image bgImage;
    public Image progressBar;
    private float _target;

    private int levelIndex = 0;
    private float winCondition = 5f;

    void Update()
    {
        if (progressBar != null)
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, _target, Time.deltaTime);
    }

    public void LoadStage(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public void ExitGame()
    {
        Application.Quit();

        // This only works in the Unity Editor, not in a standalone build
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SetWinCondition(int amount)
    {
        winCondition = amount;
        winAmountUI.text = winCondition.ToString();
    }

    public void SetLevelIndex()
    {
        levelIndex = levelDropdown.value;
        currentLevelImg.sprite = levelImages[levelIndex];
        if (levelIndex == 0)
        {
            winConditionOptions.SetActive(false);
            winAmountUI.text = "Infinite";
        }
        else
        {
            winConditionOptions.SetActive(true);
            winAmountUI.text = winCondition.ToString();
        }
    }
    public void LoadLevelByIndex()
    {
        _target = 0f;
        progressBar.fillAmount = 0;
        GameManager.winCondition = winCondition;
        bgImage.sprite = currentLevelImg.sprite;

        loaderCanvas.SetActive(true);

        StartCoroutine(LoadSceneAsync(levelIndex + 1));
    }
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneIndex);

        // Prevent the scene from activating immediately
        scene.allowSceneActivation = false;

        // Update the progress bar while the scene loads
        while (!scene.isDone)
        {
            _target = scene.progress;

            if (scene.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                scene.allowSceneActivation = true; // Activate the scene
            }

            yield return null; // Wait for the next frame
        }

        // Hide the loader canvas
        loaderCanvas.SetActive(false);
    }
}
