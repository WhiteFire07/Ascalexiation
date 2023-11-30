using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider master;
    public Slider music;
    public Slider sfx;

    public void Start()
    {
        master.value = Mathf.Pow(10, PlayerPrefs.GetFloat("masterVol")/20);
        music.value = Mathf.Pow(10, PlayerPrefs.GetFloat("musicVol")/20);
        sfx.value = Mathf.Pow(10, PlayerPrefs.GetFloat("sfxVol")/20);
    }
    public void ChangeMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
    }
    public void ChangeMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
    }
    public void ChangeSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }
    public void OnDisable()
    {
        audioMixer.GetFloat("MasterVol", out float master);
        PlayerPrefs.SetFloat("masterVol", master);
        audioMixer.GetFloat("MusicVol", out float music);
        PlayerPrefs.SetFloat("musicVol", music);
        audioMixer.GetFloat("SFXVol", out float sfx);
        PlayerPrefs.SetFloat("sfxVol", sfx);
    }
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
}
