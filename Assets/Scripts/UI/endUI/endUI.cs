using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endUI : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject Panel;

    public void showEndUIValues(bool hasWon, bool policeWon, bool gameEnded)
    {
        winPanel = GameObject.Find("winPanel");
        losePanel = GameObject.Find("losePanel");
        Panel = GameObject.Find("Panel");
        
        if (gameEnded==true)
      {
            

            if (policeWon == true)
            {
               

                Panel=winPanel;
            }
            else if (policeWon == false)
            {
                
                Panel= losePanel;
            }
            else if (hasWon == true)
            {
                
                Panel= winPanel;

            }
            else
            {
                
                Panel=losePanel;
            }
      }  
        

    }
}

