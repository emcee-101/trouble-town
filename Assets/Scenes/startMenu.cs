using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    
   public void startGame()
    {
        
        SceneManager.LoadScene(1);
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

