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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(player.pause.triggered) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            currentMenu = pauseMainMenu;
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenKeybindsMenu() {
        currentMenu.SetActive(false);
        keybinds.SetActive(true);
        currentMenu = keybinds;
    }

    public void OpenPauseMainMenu() {
        currentMenu.SetActive(false);
        pauseMainMenu.SetActive(true);
        currentMenu = pauseMainMenu;
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MenuScene");
    }
}
