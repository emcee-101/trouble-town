using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endUI : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
  

    public void showEndUIValues(bool hasWon, bool policeWon, bool gameEnded)
    {
        if (gameEnded == true)
        {

            if (hasWon)
            {
                Instantiate(winPanel);
            }
            else
            {
                Instantiate(losePanel);
            }
        }


    }
}

