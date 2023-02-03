using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActicateScoreboard : MonoBehaviour
{
    public bool activated = false;
    public GameObject scoreBoard;

    private void Awake()
    {
        scoreBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (activated) { 
            if ((Input.GetKey(KeyCode.Tab) && (!scoreBoard.active)))
            {
                scoreBoard.SetActive(true);
                
            } else if (scoreBoard.active && !Input.GetKey(KeyCode.Tab))
            {
                scoreBoard.SetActive(false);
            }
        }
    }
}
