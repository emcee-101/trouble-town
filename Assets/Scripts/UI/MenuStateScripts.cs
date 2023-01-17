using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MainMenuUIHandler;

public interface IMenu
{
    public bool toggleStatus();
}
public class MenuStateScripts : MonoBehaviour
{

    MainMenuUIHandler mainMenu;
    SettingMenuScript settingsMenu;

    public void Start()
    {
        mainMenu = GameObject.FindGameObjectWithTag("MAIN_MENU").GetComponent<MainMenuUIHandler>();
        settingsMenu = GameObject.FindGameObjectWithTag("SET_MENU").GetComponent<SettingMenuScript>();
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

        mainMenu.toggleStatus();
        settingsMenu.toggleStatus();

    }
}
