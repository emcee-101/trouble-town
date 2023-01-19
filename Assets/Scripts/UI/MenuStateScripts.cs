using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MainMenuUIHandler;

public interface IMenu
{
    public bool toggleStatus(bool setTo);
    public bool getStatus();

}
public class MenuStateScripts : MonoBehaviour
{

    MainMenuUIHandler mainMenu;
    SettingMenuScript settingsMenu;
    

    public void Start()
    {
        mainMenu = GameObject.FindGameObjectWithTag("MAIN_MENU").GetComponent<MainMenuUIHandler>();
        settingsMenu = GameObject.FindGameObjectWithTag("SET_MENU").GetComponent<SettingMenuScript>();


        mainMenu.enabled = true;
        settingsMenu.enabled = false;
    }

    public enum menuState
    {
        MAIN,
        SETTINGS
    }

    public static menuState curState = menuState.MAIN;

    public void OnSettingsClicked()
    {
        curState = (curState == menuState.SETTINGS) ? menuState.MAIN : menuState.SETTINGS;
        Debug.Log(curState);
        updateUI();
    }

    private void updateUI()
    {

        mainMenu.toggleStatus(!mainMenu.getStatus());

        // when the Object gets disabled it isnt acessible - duh -> just let tge main menu handle this ig (it overlays the settingsmenu)
        // settingsMenu.toggleStatus(!settingsMenu.getStatus());

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
