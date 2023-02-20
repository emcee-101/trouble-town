using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoring : NetworkBehaviour
{

    public float pointsForRobbing = 100;
    public float pointsForGettingCaught = -100;
    public float pointsForStoringMoney = 200;
    public float pointsForCatchingRobber = 300;
   


    [Networked, Capacity(10)]
    public NetworkDictionary<string, float> scorings { get; }
          = MakeInitializer(new Dictionary<string, float> { { "hi", 0.1f} });

    public void initScores() {
        scorings.Clear();
    }


    public void addPoints(string playerName, float pointsToAdd) {

        scorings.Set(playerName, scorings.Get(playerName) + pointsToAdd);
        if(scorings.Get(playerName) < 0.0f) { scorings.Set(playerName, 0.0f); }

        //Debug.Log("Current registered Players: " + scorings.Count);

    }

    public bool checkIfRegistered(string name)
    {

        return (scorings.ContainsKey("name"));

    }

    public void registerPlayer(string playerName)
    {
        scorings.Add(playerName, 0.0f);

    }

    public void removePlayer(string playerName)
    {

        scorings.Remove(playerName);

    }

}
