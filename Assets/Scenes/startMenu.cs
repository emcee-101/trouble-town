using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{

    public GameObject menu;
    public GameObject game;
    
   public void startGame()
    {
        // Make Mouse crosshair invisible and locked inside the game window


        menu.SetActive(false);
        game.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }
    
    public void hostGame()
    {
    }

    public void findGame()
    {
    }

}
//Scenen in Build settings in der richtigen reihenfolge festlegen, mit entsprechenden index die szene laden

