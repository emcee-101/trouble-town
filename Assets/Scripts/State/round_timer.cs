using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round_timer : NetworkBehaviour
{
    public float timeForOneRoundInSeconds;

    [Networked] public TickTimer timer { get; set; } 

    public NetworkRunner networkRunnerInScene;

    public void startTimer()
    {
        networkRunnerInScene = FindObjectOfType<NetworkRunnerHandler>().networkRunner;

        // create new timer
        timer = TickTimer.CreateFromSeconds(networkRunnerInScene, timeForOneRoundInSeconds);
    }

    public void stopTimer()
    {
        timer = TickTimer.None;
    }

    public override void FixedUpdateNetwork() {

        //if(!timer.Expired(networkRunnerInScene)) Debug.Log("has state auth: " + Object.HasStateAuthority + "  time left: " + timer.RemainingTime(networkRunnerInScene));

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
