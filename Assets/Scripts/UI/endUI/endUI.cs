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
        // do something
        if (gameEnded==true)
      {
            

            if (policeWon == true)
            {
                //Panel = GameObject.Find("winPanel");

                Panel=winPanel;
            }
            else if (policeWon == false)
            {
                //Panel=GameObject.Find("losePanel");
                Panel= losePanel;
            }
            else if (hasWon == true)
            {
                //Panel = GameObject.Find("winPanel");
                Panel= winPanel;

            }
            else
            {
                //Panel = GameObject.Find("losePanel");
                Panel=losePanel;
            }
      }  
        

    }
}

