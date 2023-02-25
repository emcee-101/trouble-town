using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ScoreEndManager : MonoBehaviour
{
    public List<ScoreEnd> scores;
    scoring scorings;


    private void Awake()
    {
        
        scores = new List<ScoreEnd>();
        scorings = GameObject.FindGameObjectWithTag("State").GetComponent<scoring>();

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

            scores.Add(new ScoreEnd(entry.Key, entry.Value));
        
        
        }

        SortScores();
    }

    private void Update()
    {
        refreshScore();
    }

}
