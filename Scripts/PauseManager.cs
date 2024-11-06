using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject rulesMenu;
    public GameObject paper;
    public static bool _isPaused;
    private TextHover[] textHoverButtons;

    void Start()
    {
        DeactivateAllMenus();
        paper.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                paper.SetActive(false);
                ResumeGame();
            }
            else 
            {   
                paper.SetActive(true);
                PauseGame();
            }

        }
        
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
        ResetButtonHoverState();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        paper.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void ShowPauseMenu()
    {
        DeactivateAllMenus();
        pauseMenu.SetActive(true);
        ResetButtonHoverState();
    }

    public void ShowMainMenu()
    {
        DeactivateAllMenus();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void ShowOptionsMenu()
    {
        DeactivateAllMenus();
        optionsMenu.SetActive(true);
        ResetButtonHoverState();
    }

    public void ShowRulesMenu()
    {
        DeactivateAllMenus();
        rulesMenu.SetActive(true);
        ResetButtonHoverState();
    }

    public void QuitGame()
    {
        // Quit the application
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void StartGame()
    {

        SceneManager.LoadScene("SampleScene");
    }

    private void DeactivateAllMenus()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        rulesMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void ResetButtonHoverState()
    {
        // Find all buttons with the TextHover component and reset their state
        TextHover[] textHoverComponents = FindObjectsOfType<TextHover>();
        foreach (var textHover in textHoverComponents)
        {
            textHover.ResetHoverState();
        }
    }
}
