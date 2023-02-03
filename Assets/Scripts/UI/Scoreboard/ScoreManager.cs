using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<Score> scores;


    private void Awake()
    {
        scores = new List<Score>();
    }

    public void SortScores()
    {
        scores.Sort();
    }

    public void refreshScore()
    {
            
    }

}
