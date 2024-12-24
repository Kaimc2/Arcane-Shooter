using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static bool muted = false;
    public static float volume = 1f;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeAmt;
    public Toggle audioToggle;

    void Start()
    {
        // Load previous settings
        muted = PlayerPrefs.GetInt("Muted", 0) != 0;
        volume = PlayerPrefs.GetFloat("Volume", 1f);

        audioToggle.isOn = muted;
        volumeAmt.text = Mathf.Round(volume * 100).ToString() + "%";
        volumeSlider.value = volume;
    }

    public void UpdateVolume()
    {
        volume = volumeSlider.value;
        volumeAmt.text = Mathf.Round(volume * 100).ToString() + "%";
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void MuteGame()
    {
        muted = audioToggle.isOn;
        AudioListener.pause = muted;
        PlayerPrefs.SetInt("Muted", muted ? 1 : 0);
    }
}
