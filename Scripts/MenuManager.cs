using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject rulesMenu;


    void Start()
    {
        // Ensure only the main menu is active when starting
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        DeactivateAllMenus();
        mainMenu.SetActive(true);
        ResetButtonHoverState();
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

    public void StartGame()
    {
 
        SceneManager.LoadScene("SampleScene"); 
    }

    public void QuitGame()
    {
        // Quit the application
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void DeactivateAllMenus()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        rulesMenu.SetActive(false);
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
