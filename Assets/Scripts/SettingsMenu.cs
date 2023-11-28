using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;

    public void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        audioMixer.SetFloat("MasterVol", Mathf.Lerp(-30, 20, slider.value));
    }
    public void ChangeVolume(float value)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Lerp(-30, 20, value));
        if (value == 0)
        {
            audioMixer.SetFloat("MasterVol", -80);
        }
    }
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
}
