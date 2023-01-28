using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class round_timer : NetworkBehaviour
{

    public float timeForOneRoundInMinutes = 10;

    [Networked] TickTimer timer { get; set; } 

    private NetworkRunner networkRunnerInScene;


    public void startTimer()
    {

        networkRunnerInScene = FindObjectOfType<NetworkRunner>();

        // create new timer
        timer = TickTimer.CreateFromSeconds(networkRunnerInScene, timeForOneRoundInMinutes * 60);


    }
    public void stopTimer()
    {

        timer = TickTimer.None;


    }

    public override void FixedUpdateNetwork() {


        // check if timer expired
        if (GetComponent<game_state>().gameState != GameState.aftergame && timer.Expired(networkRunnerInScene))
        {
            // reset timer
            stopTimer();

            // end round
            GetComponent<game_state>().gameState = GameState.aftergame;


        }

        
        // How to access the time remaning (for UI and all that)
        // time is in REMAINING Seconds
        // Debug.Log("Remaining Time: " + timer.RemainingTime(networkRunnerInScene));
       
    
    }


}
