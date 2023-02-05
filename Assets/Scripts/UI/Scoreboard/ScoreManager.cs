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
        float test = 0.0f;

        scorings = GameObject.FindGameObjectWithTag("State").GetComponent<scoring>();
        if (scorings == null) Debug.Log("Missing StateObject because Niklas did an Ooopsie");
        if (!scorings.scorings.TryGet("hi", out test)) Debug.Log("Missing TestValue bc god knows");
    }

    private void Start()
    {

    }

    public void SortScores()
    {
        scores.Sort();
    }

    public void refreshScore()
    {
        scores.Clear();
        //Debug.Log("lengst of scores after clean:" + scores.Count);

        foreach (KeyValuePair<string, float> entry in scorings.scorings) {

            scores.Add(new Score(entry.Key, entry.Value));
        
        
        }

        SortScores();
    }

}
