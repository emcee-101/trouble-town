using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<Score> scores;
    scoring scorings;


    private void Awake()
    {
        scores = new List<Score>();
    }

    private void Start()
    {
        scorings = GameObject.FindGameObjectWithTag("State").GetComponent<scoring>();
        if (scorings == null) Debug.Log("Missing StateObject because Niklas did an Ooopsie");
    }

    public void SortScores()
    {
        scores.Sort();
    }

    public void refreshScore()
    {
        scores.Clear();

        foreach (KeyValuePair<string, float> entry in scorings.scorings) {

            scores.Add(new Score(entry.Key, entry.Value));
        
        
        }

        SortScores();
    }

}
