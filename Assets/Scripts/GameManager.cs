using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseMainMenu;
    public PlayerController player;
    private GameObject currentMenu;
    public GameObject keybinds;
    public GameObject settings;

    // Start is called before the first frame update
    void Start()
    {
        currentMenu = pauseMainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.pause.triggered)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                player.Freeze();
                Time.timeScale = 0;
            } else
            {
                player.unFreeze();
                Time.timeScale = 1;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenKeybindsMenu()
    {
        currentMenu.SetActive(false);
        keybinds.SetActive(true);
        currentMenu = keybinds;
    }

    public void OpenSettingsMenu()
    {
        currentMenu.SetActive(false);
        settings.SetActive(true);
        currentMenu = settings;
    }

    public void OpenPauseMainMenu()
    {
        currentMenu.SetActive(false);
        pauseMainMenu.SetActive(true);
        currentMenu = pauseMainMenu;
    }

    public void GoToMainMenu()
    {
        Debug.Log("menu");
        SceneManager.LoadScene("MenuScene");
    }
}
