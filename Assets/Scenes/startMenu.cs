using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    
   public void startGame()
    {
        // Make Mouse crosshair invisible and locked inside the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }

}
//Scenen in Build settings in der richtigen reihenfolge festlegen, mit entsprechenden index die szene laden

