using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject info;
    public GameObject settings;
    public GameObject keybinds;
    public GameObject currentMenu;

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenInfoPanel() {
        currentMenu.SetActive(false);
        info.SetActive(true);
        currentMenu = info;
    }

    public void OpenSettings() {
        currentMenu.SetActive(false);
        settings.SetActive(true);
        currentMenu = settings;
    }

    public void OpenBasicMenu() {
        currentMenu.SetActive(false);
        mainmenu.SetActive(true);
        currentMenu = mainmenu;
    }

    public void OpenKeybindsMenu() {
        currentMenu.SetActive(false);
        keybinds.SetActive(true);
        currentMenu = keybinds;
    }
}
